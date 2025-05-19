using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using sib_api_v3_sdk.Client;
using System.Data;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Companion;


namespace CapaAdmin.Controllers
{
    public class AccountingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public AccountingController(UserManager<ApplicationUser> userManager,
			   SignInManager<ApplicationUser> signInManager,ApplicationDbContext context, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            //var query= context.Inventory.OrderByDescending(p => p.Id).Take(4).ToList();
            var query = context.Inventory.ToList();
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
            //ViewBag.ErrorMassage = "A que salga ertggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggl sol";

            string phone = configuration.GetValue<string>("ContactSettings:Phone") ?? "";// pues solo consigo el telefono 
            string Country = configuration.GetValue<string>("ContactSettings:Country") ?? "";
            string State = configuration.GetValue<string>("ContactSettings:State") ?? "";
            string Street = configuration.GetValue<string>("ContactSettings:Street") ?? "";
            string City = configuration.GetValue<string>("ContactSettings:City") ?? "";
            string ITBIS = configuration.GetValue<string>("ITBISSettings:ITBIS") ?? "";
            string ITBISCalculo = configuration.GetValue<string>("ITBISSettings:ITBISCalculo") ?? "";
            ViewBag.Phone = phone;
            ViewBag.Country = Country;
            ViewBag.City = City;
            ViewBag.State = State;
            ViewBag.Street = Street;
            ViewBag.ITBIS = ITBIS;
            ViewBag.ITBISCalculo = ITBISCalculo;
            return View();
        }




        [Authorize]
        [HttpPost]
        public IActionResult Billing(BillingDto BillingDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMassage = "A que salga ertggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggl sol";

                return View(BillingDto);
            }



            return View(BillingDto);

        }


        public IActionResult DescargarPDF()
        {
          

            //Console.WriteLine("Hello, World!");
            //// code in your main method

            //// code in your main method
            //var data = Document.Create(page =>
            //{
            //    page.Page(page =>
            //    {
            //        page.Size(PageSizes.Letter);
            //        page.Margin(2, Unit.Centimetre);
            //        page.DefaultTextStyle(x => x.FontSize(24));

            //        page.Header().ShowOnce()
            //             .Row(row =>
            //             {

            //                 row.ConstantItem(150).Height(60).Image("C://diseños//Logo proximab copy.jpg");
            //                 row.RelativeItem().Column(col => {

            //                     col.Item().AlignCenter().Text("Proxima B Multi Service").Bold().FontSize(18);
            //                     col.Item().AlignCenter().Text("C/Duarte No.07, Andrés Boca Chica").Bold().FontSize(9);
            //                     col.Item().AlignCenter().Text("Email: Proximab02@gmail.com").Bold().FontSize(9);
            //                     col.Item().AlignCenter().Text("Tel:829-643-1634").Bold().FontSize(9);

            //                 });
            //                 row.ConstantItem(105).Column(col => {

            //                     col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("RNC:000052").Bold().FontSize(11);
            //                     col.Item().BorderColor("#257272").Border(1).Background("#257272").AlignCenter().Text("Codigo Venta").Bold().FontSize(11).FontColor("#fff");
            //                     col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("No.:0051").Bold().FontSize(11);


            //                 });

            //             });

            //        page.Content().PaddingVertical(30).Column(col => {

            //            col.Item().Row(rows => {

            //                rows.ConstantItem(105).Column(col => {
            //                    col.Item().Text("Datos del cliente").Underline().Bold().FontSize(10);
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Nombre: ").SemiBold().FontSize(10);
            //                        txt.Span("ALex Ventura").FontSize(10);
            //                    });
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Cedula: ").SemiBold().FontSize(10);
            //                        txt.Span("N/A").FontSize(10);
            //                    });
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Direccion: ").SemiBold().FontSize(10);
            //                        txt.Span("N/A").FontSize(10);
            //                    });
            //                });
            //                rows.RelativeItem().Column(col => {

            //                });
            //                rows.ConstantItem(140).Column(col => {
            //                    col.Item().Text("Datos del Vendedor").Underline().Bold().FontSize(10);
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Nombre: ").SemiBold().FontSize(10);
            //                        txt.Span("Jhovanny Rivera").FontSize(10);
            //                    });
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Telefono: ").SemiBold().FontSize(10);
            //                        txt.Span("829-643-16354").FontSize(10);
            //                    });
            //                    col.Item().Text(txt =>
            //                    {
            //                        txt.Span("Direccion: ").SemiBold().FontSize(10);
            //                        txt.Span("N/A").FontSize(10);
            //                    });
            //                });

            //            });
            //            col.Item().PaddingVertical(15);
            //            col.Item().AlignRight().Text("Fecha: " + DateTime.Now.ToString("dd/mm/yyyy")).FontSize(12);
            //            //linea
            //            col.Item().PaddingVertical(30).LineHorizontal(0.5f);

            //            col.Item().Table(tabla =>
            //            {
            //                tabla.ColumnsDefinition(columns =>
            //                {
            //                    columns.RelativeColumn(3);
            //                    columns.RelativeColumn();
            //                    columns.RelativeColumn();
            //                    columns.RelativeColumn();
            //                });

            //                tabla.Header(heather => {

            //                    heather.Cell().Background("#257272").
            //                         Padding(2).Text("Producto").FontSize(14).FontColor("#fff").Bold();

            //                    heather.Cell().Background("#257272").
            //                         Padding(2).Text("Precio Unid").FontSize(14).FontColor("#fff").Bold();

            //                    heather.Cell().Background("#257272").
            //                         Padding(2).Text("Cantidad").FontSize(14).FontColor("#fff").Bold();

            //                    heather.Cell().Background("#257272").
            //                         Padding(2).Text("Total").FontSize(14).FontColor("#fff").Bold();
            //                });

            //                foreach (var item in Enumerable.Range(1, 45))
            //                {
            //                    var cantidad = Placeholders.Random.Next(1, 10);

            //                    var precio = Placeholders.Random.Next(5, 15);

            //                    var total = cantidad * precio;


            //                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(Placeholders.Label()).FontSize(10);
            //                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(cantidad.ToString()).FontSize(10);
            //                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"$/{precio}").FontSize(10);
            //                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"$/{total}").FontSize(10);

            //                }

            //            });

            //            col.Item().AlignRight().Text("Total:1500").FontSize(12);
            //            col.Item().PaddingVertical(3);
            //            col.Item().Background(Colors.Grey.Lighten2)
            //            .Padding(10).Column(column =>
            //            {
            //                column.Item().Text("Comentario").FontSize(14);
            //                column.Item().Text("No aceptamos devoluciones, rebice su compra y/o documentos antes de irse").FontSize(10);
            //                column.Spacing(10);

            //            });


            //        });

            //        page.Footer()

            //            .Height(75)
            //            .AlignRight()
            //            .AlignMiddle()
            //            .Text(txt =>
            //            {

            //                txt.Span("Pagina ").FontSize(10);
            //                txt.CurrentPageNumber().FontSize(10);
            //                txt.Span(" de ").FontSize(10);
            //                txt.TotalPages().FontSize(10);
            //            });
            //    });
            //});

            //// instead of the standard way of generating a PDF file
            //data.GeneratePdf("hello.pdf");

            //// use the following invocation
       

            return View();
       }


    }
}