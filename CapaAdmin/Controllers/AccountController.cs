using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IConfiguration configuration;
		private readonly IEmailSender emailSender;

		//private readonly IEmailSender emailSender;

		public AccountController(UserManager<ApplicationUser> userManager,
			   SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailSender emailSender )
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.configuration = configuration;
			this.emailSender = emailSender;
		}


		public IActionResult Register()
		{

			if (signInManager.IsSignedIn(User)){

			
				return RedirectToAction("Index", "Home");
			}

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto registerDto)
		{
			if (!ModelState.IsValid)
			{

				return View(registerDto);
			}
		
			var user = new ApplicationUser()
			{
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				Email = registerDto.Email,	
				UserName=registerDto.Email,
				PhoneNumber = registerDto.PhoneNumber,	
				Address = registerDto.Address,	
				CreatedAt = DateTime.Now,
			};

			var result = await userManager.CreateAsync(user, registerDto.Password);
			
			if (result.Succeeded) {
				await userManager.AddToRoleAsync(user, "Client");
				await signInManager.SignInAsync(user, false);

			   return RedirectToAction("Index","Home");

			}
			foreach (var error in result.Errors) {

				ModelState.AddModelError("", error.Description);
			}


			return View(registerDto);
		}

        public async Task<IActionResult> Logout()
        {
			if (signInManager.IsSignedIn(User))
			{
				await signInManager.SignOutAsync();

			}

            return RedirectToAction("Index", "Home");

        }
		public IActionResult Login()
		{


			if (signInManager.IsSignedIn(User))
			{


				return RedirectToAction("Index", "Home");
			}

			return View();
		}
		[HttpPost]
			public async Task<IActionResult> Login(LoginDto loginDto)
        {

			if (signInManager.IsSignedIn(User))
			{


				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{

				return View(loginDto);
			}
			var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, 
				loginDto.RememberMe,false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");

			}
			else
			{
				ViewBag.ErrorMassage = "Invalid login";
			}

			return View();
        }
		// if you are no loing the go you at login
		[Authorize]
        public async Task<IActionResult> Profile()
        {
			var appUser = await userManager.GetUserAsync(User);
			if (appUser == null) {

				return RedirectToAction("Index", "Home");
			}

			var profileDto= new ProfileDto()
			{
				FirstName = appUser.FirstName,
				LastName = appUser.LastName,
				Email = appUser.Email ?? "",
				PhoneNumber = appUser.PhoneNumber,
				Address = appUser.Address

			};

            return View(profileDto);
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Profile(ProfileDto profileDto)
		{

			if (!ModelState.IsValid) {
				ViewBag.ErrorMassage = "Plese Fill all the required fields with valid values";
				return View(profileDto);

			}
			var appUser= await userManager.GetUserAsync(User);
			if (appUser == null) {


				return RedirectToAction("Index", "Home");
			}

			// Upsatthe user profile
			appUser.FirstName = profileDto.FirstName;
			appUser.LastName = profileDto.LastName;
			appUser.Email = profileDto.Email;
			appUser.UserName = profileDto.Email;
			appUser.PhoneNumber = profileDto.PhoneNumber;
			appUser.Address = profileDto.Address;

			var result= await userManager.UpdateAsync(appUser);// update in database

			if (result.Succeeded)
			{
				ViewBag.SuccessMessage = "Profile update Successfully";

			}
			else {
				ViewBag.ErrorMassage = "Unable to update the profile"+ result.Errors.First().Description;

			}

			return View(profileDto);
		}

		[Authorize]
		public IActionResult Password()
		{
			if (!signInManager.IsSignedIn(User))
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		[Authorize]
		public  async Task<IActionResult> Password(PasswordDto passwordDto)
		{
			if (!ModelState.IsValid)
			{

				ViewBag.ErrorMassage = "Plese Fill all the required fields with valid values";
				return View(passwordDto);

			}

			var appUser = await userManager.GetUserAsync(User);
			if (appUser == null)
			{

				return RedirectToAction("Password", "Account");
			}

			// Upsatthe user profile


			var result = await userManager.ChangePasswordAsync(appUser,passwordDto.CurrentPassword, passwordDto.NewPassword);// update in database

			if (result.Succeeded)
			{
				ViewBag.SuccessMessage = "Password update Successfully";
			}
			else
			{
				ViewBag.ErrorMassage = "Unable to update the Passwrod " + result.Errors.First().Description;

			}

			return View(passwordDto);
		}


		public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");

        }

		public IActionResult ForgotPassword()
		{

			if (signInManager.IsSignedIn(User))
			{

				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword([Required, EmailAddress] string email)
		{

			if (signInManager.IsSignedIn(User))
			{

				return RedirectToAction("Index", "Home");
			}
			ViewBag.Email= email;

			if (!ModelState.IsValid)
			{

				ViewBag.ErrorMassage = ModelState["email"]?.Errors.First().ErrorMessage ?? "Invalid Email Addres ";
				return View();

			}

			var user = await userManager.FindByEmailAsync(email);// update in database

			if (user != null)
			{
				//generate password reset token
				var token = await userManager.GeneratePasswordResetTokenAsync(user);
				 string resetUrl= Url.ActionLink("ResetPassword","Account", new { token }) ?? "URL Error";
				//string senderName2 = configuration["EmailSettings:SenderName"] ?? "";
				string senderName = configuration.GetValue<string>("EmailSettings:SenderName") ?? "";
				string pw = configuration.GetValue<string>("EmailSettings:Apikey")  ?? "";
				string senderEmail = configuration.GetValue<string> ("EmailSettings:SenderEmail") ?? "";

				string username= user.FirstName +"  "+user.LastName;
				string subject="Password Reset";

				string message = "Dear "+username+", \n\n" +
					" You can reset your password using the follwing link: \n\n" + resetUrl+ " \n\n"+
					"ShopingDoping "+ subject;
			//	Console.WriteLine(senderEmail);
				//Console.WriteLine(senderEmail+" este Contras "+ pw+ "y este senderName "+ senderName2 + " este usuario email " +email + message);

				await emailSender.SendEmailAsync(senderEmail, pw,email, username, message);
				//	return RedirectToAction("Account", "ForgotPassword");

			}

			ViewBag.SuccessMessage = "Please chek your email Account and click on the password Reset Link !";


			return View();
		}


		public IActionResult ResetPassword(string? token)
		{

			if (signInManager.IsSignedIn(User))
			{

				return RedirectToAction("Index", "Home");
			}
			if(token == null){

				return RedirectToAction("Index", "Home");
			}


			return View();
		}

		[HttpPost]
		public async  Task<IActionResult> ResetPassword(string? token, PasswordResetDto model)
		{

			if (signInManager.IsSignedIn(User))
			{

				return RedirectToAction("Index", "Home");
			}
			if (token == null)
			{

				return RedirectToAction("Index", "Home");
			}
			if(!ModelState.IsValid)
			{

				//ViewBag.ErrorMassage = "Plese Fill all the required fields with valid values";
				return View(model);

			}

			var user = await userManager.FindByEmailAsync(model.Email);

			if (user == null) {

				ViewBag.ErrorMassage = "Token Not valid";
				return View(model);


			}

			var result = await userManager.ResetPasswordAsync(user,token,model.Password);

			if (result.Succeeded) {
				ViewBag.SuccessMessage = "Password update Successfully";
			}
			else
			{

				foreach (var error in result.Errors) { 
				
				
				   ModelState.AddModelError("", error.Description);
				}

				ViewBag.ErrorMassage = "Token Not valid";
			}


			return View(model);
		}


	}
}
