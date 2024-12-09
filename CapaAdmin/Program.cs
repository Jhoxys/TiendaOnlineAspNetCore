using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Client;
using System.Net;
using System.Reflection.PortableExecutable;

internal class Program
{
	private static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddTransient<IEmailSender, EmailSender>();

		// Add services to the container.
		builder.Services.AddControllersWithViews();
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
		{

			var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
			options.UseSqlServer(connectionStrings);
		});

		builder.Services.AddIdentity<ApplicationUser, IdentityRole>(

			options =>
			{
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;

			}).AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders(); // token usado en la action  forgot password




		//Configuration.Default.ApiKey.Add("api-key", builder.Configuration["EmailSettings:Apikey"]);


		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		// create the roles and the fist admin user if not avaiblable yet

		using (var scope = app.Services.CreateScope())
		{

			var userMnager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
				as UserManager<ApplicationUser>;

			var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
			   as RoleManager<IdentityRole>;

			await DatabaseInitializer.SeedDataAsync(userMnager, roleManager);

		}

		app.Run();
	}
}