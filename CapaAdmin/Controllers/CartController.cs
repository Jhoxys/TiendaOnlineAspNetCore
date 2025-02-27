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
            ViewBag.Total = subtotal + shippingFee;



            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Index(ChekckoutDto model)
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
            TempData["DeliveryAddress"] = model.DeliveryAddreess;
            TempData["PaymentMethod"] = model.PaymentMethod;


            return RedirectToAction("Confirm");


        }
        public IActionResult Confirm()
        {
            //OrderItem
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            decimal total = CartHelper.GetSubtotal(cartItems) + shippingFee;

            int cartSize = 0;
            foreach (var items in cartItems) {

                cartSize += items.Quantity;



            }

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();


            if (cartSize ==0  || deliveryAddress.Length==0 || paymentMethod.Length ==0)
            {
                return RedirectToAction("Index", "Home");
            }

			ViewBag.DeliveryAddress = deliveryAddress;
			ViewBag.PaymentMethod = paymentMethod;
			ViewBag.Total = total;
			ViewBag.CartSize = cartSize;
			return View();
		}
        [Authorize]
        [HttpPost]
		public async Task<IActionResult> Confirm(int any)
		{
			//OrderItem
            var cartItems = CartHelper.GetCartItems(Request,Response, context);
			string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
			string paymentMethod = TempData["PaymentMethod"] as string ?? "";
			TempData.Keep();
			if (cartItems.Count == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
			{
				return RedirectToAction("Index", "Home");
			}

             var appUser = await userManager.GetUserAsync(User);    
            if(appUser == null)
             {  return RedirectToAction("Index", "Home");}
			var oder = new Order
			{
			    ClientId= appUser.Id,
                Items = cartItems,
                ShippingFree=shippingFee,
                DeliveryAddres=deliveryAddress,
                PaymentMethod=paymentMethod,
				PaymentStatus = "pending",
				PaymentDetails = "",
				OrderStatus = "created",
                CreatedAt = DateTime.Now,



			};
            //// nombre de la referencia de la tabla y la order llena filas
            context.Orders.Add(oder);
            context.SaveChanges();
            Response.Cookies.Delete("shopping_cart");
			ViewBag.SuccesMessage= "Order Created Succesfully";
			return View();

		}
	}// main
	}// namespace