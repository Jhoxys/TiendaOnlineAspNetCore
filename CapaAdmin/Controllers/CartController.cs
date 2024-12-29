using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CapaAdmin.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly decimal shippingFee;
        public CartController(UserManager<ApplicationUser> userManager,
               ApplicationDbContext context, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.context = context;
            this.configuration = configuration;
                shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
