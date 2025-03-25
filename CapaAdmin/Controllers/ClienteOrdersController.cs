using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Controllers
{
	[Authorize(Roles = "client")]
	[Route("/Client/ORders/{action=Index}/{id?}")]
	public class ClienteOrdersController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly int pageSize = 5;
		public ClienteOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
			this.context = context;
			this.userManager = userManager;
		}

		public IActionResult Index(int pageIndex)
		{
			IQueryable<Order> query = context.Orders.Include(o => o.Client)
				.Include(o => o.Items).OrderByDescending(o => o.Id);


			if (pageIndex <= 0)
			{
				pageIndex = 1;
			}

			decimal count = query.Count();
			int totalPages = (int)Math.Ceiling(count / pageSize);

			query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

			var oders = query.ToList();

			ViewBag.Orders = oders;
			ViewBag.PageIndex = pageIndex;
			ViewBag.totalPages = totalPages;


			return View();
		}
	}
}
