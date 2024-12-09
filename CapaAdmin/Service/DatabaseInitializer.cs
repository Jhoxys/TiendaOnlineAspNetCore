using CapaAdmin.Models;
using Microsoft.AspNetCore.Identity;

namespace CapaAdmin.Service
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync (UserManager<ApplicationUser>? userMnager,
              RoleManager<IdentityRole>? roleManager)
        {
            if (userMnager == null || roleManager == null)
            {

                Console.WriteLine("SUer manager or role is null exit");
                return;
            }

            var exits = await roleManager.RoleExistsAsync("admin");
            if (!exits)
            {
                Console.WriteLine("Admin role is not defined and wilbe created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            // check if we have the seller role or not
             exits = await roleManager.RoleExistsAsync("seller");
            if (!exits)
            {
                Console.WriteLine("seller role is not defined and wilbe created");
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }

            // check if we have the clinet role or not
            exits = await roleManager.RoleExistsAsync("client");
            if (!exits)
            {
                Console.WriteLine("seller role is not defined and wilbe created");
                await roleManager.CreateAsync(new IdentityRole("client"));
            }


            var adminUser = await userMnager.GetUsersInRoleAsync("admin");  // userNamnaer and Any to see exits
            if (adminUser.Any())
            {
                Console.WriteLine("Admin role is not defined and wilbe created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }


            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                CreatedAt =  DateTime.Now

            };

            string initialPassoword = "admin123";// default 1 time

            var result= await userMnager.CreateAsync(user, initialPassoword);

            if (result.Succeeded)
            {

                  await userMnager.AddToRoleAsync(user,"admin");
                   Console.WriteLine("Admin user created successfuly please update the initial password");
                  Console.WriteLine("Email: "+ user.Email);
                Console.WriteLine("Initial Password: " + initialPassoword);
            }


        }

    }
}
