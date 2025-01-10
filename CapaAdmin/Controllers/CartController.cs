using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
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
            // OrderItem
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            decimal subtotal = CartHelper.GetSubtotal(cartItems);
            ViewBag.CartItems = cartItems;  
            ViewBag.ShippinFee = shippingFee;    
            ViewBag.Subtotal = subtotal;
            ViewBag.Total= subtotal + shippingFee;



            return View();
        }
        [Authorize]
        [HttpPost]
		public IActionResult Index( ChekckoutDto model)
		{
			// OrderItem
			List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
			decimal subtotal = CartHelper.GetSubtotal(cartItems);
			ViewBag.CartItems = cartItems;
			ViewBag.ShippinFee = shippingFee;
			ViewBag.Subtotal = subtotal;
			ViewBag.Total = subtotal + shippingFee;


			if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (cartItems.Count == 0) {

                ViewBag.ErrorMassage = "Your cart is empty";
				return View(model);
			}
            //pasar datos a esa Action en un redirect
            TempData["DeliveryAddress"]= model.DeliveryAddreess;
			TempData["PaymentMethod"] = model.PaymentMethod;


			return RedirectToAction("Confirm");


		}
	}
}
