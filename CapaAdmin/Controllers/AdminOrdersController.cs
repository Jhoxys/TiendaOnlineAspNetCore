using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/Admin/Orders/{action=index}/{id?}")]
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int  pageSize =5;
        public AdminOrdersController(ApplicationDbContext context) {
            this.context = context;
        }

        public IActionResult Index(int pageIndex)
        {
            IQueryable<Order> query= context.Orders.Include(o => o.Client)
                .Include(o => o.Items).OrderByDescending(o => o.Id);

           
            if(pageIndex <= 0)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages= (int)Math.Ceiling(count/pageSize);

            query = query.Skip((pageIndex-1) * pageSize).Take(pageSize);

            var oders = query.ToList();

            ViewBag.Orders = oders;
            ViewBag.PageIndex = pageIndex;
            ViewBag.totalPages = totalPages;


            return View();
        }


        public IActionResult Details(int pageIndex, int? id)
        {


           var orders= context.Orders.Include(o => o.Client)
                .Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefault(o=> o.Id == id) ;

          
            if (orders == null)
            {
                return RedirectToAction("index");
            }

            ViewBag.NumOrders= context.Orders.Where(o=>o.ClientId== orders.ClientId ).Count();
            return View(orders);
        }



		public IActionResult Edit( int? id, string? payment_status, string? order_status)
		{
            var order = context.Orders.Find(id);
            if (order == null) {

				return RedirectToAction("index");
			}

            if (payment_status == null && order_status == null) {

				return RedirectToAction("Details", new { id });


			}
			if (payment_status != null)
			{
				order.PaymentStatus= payment_status;
			}


			if (order_status != null)
			{

				order.OrderStatus = order_status;
			}
			context.SaveChanges(); 

			return RedirectToAction("Details", new { id });


		}
	}
}
