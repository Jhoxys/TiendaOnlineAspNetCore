using CapaAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CapaAdmin.Controllers
{

	[Authorize(Roles = "admin")]
	[Route("/Admin/[controller]/{action=Index}/{id?}")]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly int pageSize = 3;

		public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
		}
		public IActionResult Index(int? pageIndex)
		{
			IQueryable<ApplicationUser> query = userManager.Users.OrderByDescending(x => x.CreatedAt);
			// pagination funtionality
			if (pageIndex == null || pageIndex < 1)
			{
				pageIndex = 1;
			}

			decimal count = query.Count();

			int totalPages = (int)Math.Ceiling(count / pageSize);

			query = query.Skip(((int)pageIndex - 1) * pageSize).Take(pageSize);

			var users = query.ToList();

			ViewBag.PageIndex = pageIndex;
			ViewBag.TotalPages = totalPages;

			return View(users);
		}

		public async Task<IActionResult> Details(string? Id)
		{

			if (Id == null) {

				return RedirectToAction("Index", "Users");

			}

			var appUser = await userManager.FindByIdAsync(Id);

			if (appUser == null)
			{

				return RedirectToAction("Index", "Users");

			}

			ViewBag.Roles = await userManager.GetRolesAsync(appUser);

			var awailableRole = roleManager.Roles.ToList();
			var item = new List<SelectListItem>();

			foreach (var role in awailableRole)
			{
				item.Add(new SelectListItem {
					Text = role.NormalizedName,
					Value = role.Name,
					Selected = await userManager.IsInRoleAsync(appUser, role.Name!)
				});
			}
			ViewBag.SelectItems = item;

			return View(appUser);
		}

		public async Task<IActionResult> EditRole(string? id, string? newRole)
		{
			// no  tiene view
			if (id == null || newRole == null)
			{

				return RedirectToAction("Index", "Users");

			}

			var roleExists= await roleManager.RoleExistsAsync(newRole);
			var appUser = await userManager.FindByIdAsync(id); 

			if(appUser == null ||  !roleExists) return RedirectToAction("Index", "Users");


			var correntUser = await userManager.GetUserAsync(User);
			 if(correntUser!.Id == appUser.Id)
			{
				TempData["ErrorMessage"] = "Yu cannot udate your own Role!";// mensajes para cookies
				return RedirectToAction("Details", "Users", new { id });
			}

			// update nuse role

			var userRoles = await userManager.GetRolesAsync(appUser);
			await userManager.RemoveFromRolesAsync(appUser, userRoles);
			await userManager.AddToRoleAsync(appUser, newRole);
			TempData["SuccessMessage"] = "User Role update sucessfully";// mensajes para cookies


			return RedirectToAction("Details", "Users", new { id });

		}

		public async Task<IActionResult> DeleteAccount(string? id)
		{

			if (id == null )
			{
				return RedirectToAction("Index", "Users");

			}

			var appUser= await userManager.FindByIdAsync(id);

			if(appUser ==null) return RedirectToAction("Index", "Users");


			var correntUser = await userManager.GetUserAsync(User);
			if (correntUser!.Id == appUser.Id)
			{
				TempData["ErrorMessage"] = "Yu cannot delete your own Role!";// mensajes para cookies
				return RedirectToAction("Details", "Users", new { id });
			}

			// delete

			var result = await userManager.DeleteAsync(appUser);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Users");
			}

			TempData["ErrorMessage"] = "Unable to delete this account: "+ result.Errors.First().Description; // mensajes para cookies

			return RedirectToAction("Details", "Users", new { id });

		}

	}
	}
