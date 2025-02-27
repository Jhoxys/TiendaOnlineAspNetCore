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

        public AdminOrdersController(ApplicationDbContext context) {
            this.context = context;
        }

        public IActionResult Index()
        {
            var oders= context.Orders.Include(o => o.Client)
                .Include(o => o.Items).OrderByDescending(o=> o.Id).ToList();

             ViewBag.Orders = oders;

            return View();
        }
    }
}
