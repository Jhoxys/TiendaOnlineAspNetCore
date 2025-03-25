using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CapaAdmin.Controllers
{
    public class AccountingController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountingController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            //var query= context.Inventory.OrderByDescending(p => p.Id).Take(4).ToList();
            var   query = context.Inventory.ToList();
            decimal contar = 0;
            DateTime fecha = DateTime.Now;

            //  ganancias diarias / DailyEarnings
            var hoy = query.Where(o => o.CreatedAt.Day == fecha.Day);
            decimal hoys = 0;
            if (hoy.Count() > 0)
            {
                foreach (var item in query)
                {
                    hoys += item.DailyEarnings;
                }
            }
            //  ganancias diarias / DailyEarnings
            var month = query.Where(o => o.CreatedAt.Month == fecha.Month);
            decimal months = 0;
            if (month.Count() > 0)
            {
                foreach (var item in query)
                {
                    months += item.MonthlyEarnings;
                }
            }
            //  ganancias mensuales / DailyEarnings
            var year = query.Where(o => o.CreatedAt.Year == fecha.Year);
            decimal years = 0;
            if (month.Count() > 0)
            {
                foreach (var item in query)
                {
                    years += item.YearEarnings;
                }
            }

            //  ganancias mensuales / DailyEarnings
            var week = query.Where(o => o.CreatedAt.DayOfWeek == fecha.DayOfWeek);
            decimal weeks = 0;
            if (week.Count() > 0)
            {
                foreach (var item in query)
                {
                    weeks += item.WeeklyEarnings;
                }
            }


            ViewBag.Today = hoys;
            ViewBag.Month = months;
            ViewBag.Years = years;
            ViewBag.Weeks = weeks;

            return View(query);
        }

        public IActionResult Billing()
        {

            return View();
        }
    }
}
