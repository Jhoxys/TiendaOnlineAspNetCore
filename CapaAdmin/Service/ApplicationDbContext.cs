using CapaAdmin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Service
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        } 

        public DbSet<Product>Products { get; set; }
    
		public DbSet<Order> Orders{ get; set; }
        public DbSet<Billing> Billing { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Typing> Typing { get; set; }
    }

}