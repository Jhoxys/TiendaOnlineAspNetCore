using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using sib_api_v3_sdk.Client;
using System.Data;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using QuestDocument = QuestPDF.Fluent.Document;

using System.Xml.Linq;
using QuestPDF.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using QuestPDF.Previewer;
using System.IO.Pipelines;
using Document = QuestPDF.Fluent.Document;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CapaAdmin.Controllers
{
    public class AccountingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public AccountingController(UserManager<ApplicationUser> userManager,
			   SignInManager<ApplicationUser> signInManager,ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.configuration = configuration;
            this.env = env;
        }

        public IActionResult Index()
        {
            //var query= context.Inventory.OrderByDescending(p => p.Id).Take(4).ToList();
            var query = context.Inventory.ToList();
         //   decimal contar = 0;
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


            var random = new Random();
            var numero = random.Next(1000, 9999); // 4 dígitos aleatorios
            string fecha = DateTime.Now.ToString("yyMds"); // Fecha y hora
                       
            var fa=$"{numero}-{fecha}";

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
            ViewBag.NoFactura = fa;
            ViewBag.ITBISCalculo = ITBISCalculo;
            Clients clients = new Clients();
            ViewData["ClientsFirstName"] = clients.FirstName;



            return View();
        }




        [Authorize]
        [HttpPost]
        public IActionResult  Billing(FacturaDto fact)
        {
            decimal totalGeneral = 0;
            decimal ITBValid = 0;

            string ITBIS = configuration.GetValue<string>("ITBISSettings:ITBIS") ?? "";
            decimal ITBISCalculo = configuration.GetValue<decimal>("ITBISSettings:ITBISCalculo");
            if (!ModelState.IsValid)
            {
                //{
              
       
        foreach(var item in fact.BillingDto)
                {

                   if( item.Quantitys > 0)
                    {
                        //if (item.Quantitys <= 0)
                        //{
                        //    ModelState.AddModelError("Quantitys", "La cantidad debe ser mayor a 0");
                        //    return View();
                        //}
                        //if (item.CodeProduct <= 0)
                        //{
                        //    ModelState.AddModelError("CodeProduct", "El codigo del producto debe ser mayor a 0");
                        //    return View();
                        //}
                        //if (string.IsNullOrEmpty(item.Description))
                        //{
                        //    ModelState.AddModelError("Description", "La descripcion no puede estar vacia");
                        //    return View();
                        //}
                        var product = context.Products.Find(item.ProductId);

                        if (product != null)
                        {
                            // Descontar stock si existe
                            product.Stock -= item.Quantitys;


                        }
                        else
                        {
                           
                   
                        }
                        

                        Billing billings = new Billing()
                    {
                    
                        Description = item.Description,
                        NoFactura =  item.NoFactura,
                        CodeProduct = item.CodeProduct,
                        Discount = item.Discount,
                        ITB = item.ITB,
                        Total = item.Total,
                        Checks= item.Checks,
                            Quantity = item.Quantitys,
                        CreatedAt = DateTime.Now,


                        Product  = new Product()
                         {
                        Name = item.Name ?? "Digitacion",
                            Brand = item.Brand,
                            Category = item.Category,
                            Price = item.Price,
                            Description = item.Description,
                            CodeProduct = item.CodeProduct 
                           }
                        };

               


                        context.Billing.Add(billings); //  inserta
   
                    }
                }        
                // Aquí insertas en BD y descontas stock…

                // vcontext.Bil
                context.SaveChanges();

                ViewBag.SuccessMessage = "Se envio la factura sactifactoriamente!";
                // validar la fucking licencia 
                QuestPDF.Settings.License = LicenseType.Community;
                // // validar la fucking licencia 

                QuestPDF.Settings.EnableDebugging = true;
                //// code in your main method
               var data = Document.Create(container =>
                {



                    container.Page(page =>
                    {
                        page.Size(80, 150); // mm
                        page.Margin(5);
                        page.Header().ShowOnce()
                        .Row(row =>
         {

             var webRoot = env.WebRootPath; // necesitas IWebHostEnvironment _env inyectado
             var imagePath = Path.Combine(webRoot, "img", "Logo proximab copy.jpg");
             //row.RelativeItem().Image("C://diseños//Logo proximab copy.jpg");
             row.RelativeItem().Column(col =>
             {
                 col.Item().AlignCenter().Height(20). Image(imagePath);
                 col.Item().AlignCenter().Text("Proxima B Multi Service").Bold().FontSize(6);
                 col.Item().AlignCenter().Text("C/Duarte No.07, Andrés Boca Chica").Bold().FontSize(3);
                 col.Item().AlignCenter().Text("Email: Proximab02@gmail.com").Bold().FontSize(3);
                 col.Item().AlignCenter().Text("Tel:829-643-1634").Bold().FontSize(3);
                
             });
           

         });


        page.Content().Column(column =>
                 {
                   column.Item().PaddingVertical(2);
                   column.Item().Text("Factura").FontSize(5).Bold();
                   column.Item().AlignRight(). Text("Fecha: "+DateTime.Now.ToString("d") ).FontSize(3).Bold();
                   column.Item().PaddingVertical(2);
                   column.Item().Table(tabla =>
                              {
                                  tabla.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn(2);
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                   });
                                  tabla.Header(heather =>
                                     {

                                         heather.Cell().Background("#257272").
                                               Padding(2).Text("Producto").FontSize(3).FontColor("#fff").Bold();

                                         heather.Cell().Background("#257272").
                                               Padding(2).Text("P/Unid").FontSize(3).FontColor("#fff").Bold();

                                         heather.Cell().Background("#257272").
                                               Padding(2).Text("Cant").FontSize(3).FontColor("#fff").Bold();

                                         heather.Cell().Background("#257272").
                                               Padding(2).Text("Total").FontSize(3).FontColor("#fff").Bold();


                                     });
                                  foreach (var item in fact.BillingDto)
                                  {
                                      if (item.Quantitys <= 0)
                                      {

                                      }
                                      else
                                      {
                                          if (item.Checks > 0)
                                          {
                                              ITBValid++;
                                          }


                                          var cantidad = item.Quantitys;

                                          var precio = item.Price;

                                          var total = cantidad * precio;
                                          totalGeneral += total;
                                          tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.Description + " dd " + ITBValid).FontSize(3);
                                          tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(cantidad.ToString()).FontSize(3);
                                          tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"${precio}").FontSize(3);
                                          tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"${total}").FontSize(3);
                                      }

                                  }
                                  var CalITBIS = totalGeneral * ITBISCalculo;
                                  var SumaTotalITB = totalGeneral + CalITBIS;
                                  column.Item().PaddingVertical(2);
                                  column.Item().AlignRight().Text("Sub total:$" + totalGeneral).FontSize(3).Bold();
                                  if (ITBValid > 0)
                                  {
                                      column.Item().AlignRight().Text($"ITB({ITBIS}%)$" + CalITBIS).FontSize(3).Bold();
                                      column.Item().AlignRight().Text("Total:" + SumaTotalITB).FontSize(3).Bold();

                                  }
                                  else { column.Item().AlignRight().Text("Total:" + totalGeneral).FontSize(3).Bold(); }
                  
                                 


                              });
                        });



                        page.Footer().Column(page =>
                        {
                            page.Item().PaddingVertical(2).LineHorizontal(0.5f);
                            page.Item().AlignRight().Text(txt =>
                            {
                                txt.Span("Pagina ").FontSize(3);
                                txt.CurrentPageNumber().FontSize(3);
                                txt.Span(" de ").FontSize(3);
                                txt.TotalPages().FontSize(3);
                            });
                            page.Item().PaddingVertical(2).Text("Gracias por su compra!").FontSize(3).Bold();
                            page.Item().Text("No aceptamos devoluciones, rebice su compra y/o documentos antes de irse").FontSize(3);
                        });



                    });
                })

               // instead of the standard way of generating a PDF file
               .GeneratePdf();

                Stream stream = new MemoryStream(data);

                return File(stream, "application/pdf", $"Fact-{DateTime.Now}.pdf");
                //return View(); //return RedirectToAction("Billing");
            }
            else
            {
                ViewBag.ErrorMessage = "El modelo no es válido. xd";
            }


            return View();


        }


        public IActionResult DescargarPDF()
        {








            //.Decoration(decoration =>
            // {
            //     decoration.Before().Column(column =>
            //     {
            //         column.Item()
            //             .ShowOnce()
            //             .Row(row =>
            //             {
            //                 row.ConstantItem(80).AspectRatio(4 / 3f).Placeholder();
            //                 row.ConstantItem(10);
            //                 row.RelativeItem()
            //                     .AlignMiddle()
            //                     .Column(innerColumn =>
            //                     {
            //                         innerColumn.Item().Text("Invoice #1234").FontSize(24).Bold();
            //                         innerColumn.Item().Text($"Generated on {DateTime.Now:d}").FontSize(16).Light();
            //                     });
            //             });
              
            //         column.Item()
            //             .SkipOnce()
            //             .Text("Invoice #1234").FontSize(24).Bold();
            //     });

            // generate dummy content
            //decoration.Content()
            //    .PaddingTop(15)
            //    .ExtendHorizontal()
            //    .Column(column =>
            //    {
            //        column.Spacing(10);

            //        foreach (var i in Enumerable.Range(1, 15))
            //        {
            //            column.Item()
            //                .Height(30)
            //                .Background(Colors.Grey.Lighten3)
            //                .AlignCenter()
            //                .AlignMiddle()
            //                .Text($"{i}");
            //        }
            //    });







            // // validar la fucking licencia 
            // QuestPDF.Settings.License = LicenseType.Community;

            // //// code in your main method
            // var data = QuestPDF.Fluent.Document.Create(page =>
            // {
            //     page.Page(page =>
            //     {
            //         page.Size(PageSizes.Letter);
            //         page.Margin(2, Unit.Centimetre);
            //         page.DefaultTextStyle(x => x.FontSize(24));
            //         page.Header().ShowOnce()
            //              .Row(row =>
            //              {

            //                  row.ConstantItem(150).Height(60).Image("C://diseños//Logo proximab copy.jpg");
            //                  row.RelativeItem().Column(col => {

            //                      col.Item().AlignCenter().Text("Proxima B Multi Service").Bold().FontSize(18);
            //                      col.Item().AlignCenter().Text("C/Duarte No.07, Andrés Boca Chica").Bold().FontSize(9);
            //                      col.Item().AlignCenter().Text("Email: Proximab02@gmail.com").Bold().FontSize(9);
            //                      col.Item().AlignCenter().Text("Tel:829-643-1634").Bold().FontSize(9);

            //                  });
            //                  row.ConstantItem(105).Column(col => {

            //                      col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("RNC:000052").Bold().FontSize(11);
            //                      col.Item().BorderColor("#257272").Border(1).Background("#257272").AlignCenter().Text("Codigo Venta").Bold().FontSize(11).FontColor("#fff");
            //                      col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("No.:0051").Bold().FontSize(11);


            //                  });

            //              });
            //         page.Content().PaddingVertical(30).Column(col => {

            //             col.Item().Row(rows => {

            //                 rows.ConstantItem(105).Column(col => {
            //                     col.Item().Text("Datos del cliente").Underline().Bold().FontSize(10);
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Nombre: ").SemiBold().FontSize(10);
            //                         txt.Span("ALex Ventura").FontSize(10);
            //                     });
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Cedula: ").SemiBold().FontSize(10);
            //                         txt.Span("N/A").FontSize(10);
            //                     });
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Direccion: ").SemiBold().FontSize(10);
            //                         txt.Span("N/A").FontSize(10);
            //                     });
            //                 });
            //                 rows.RelativeItem().Column(col => {

            //                 });
            //                 rows.ConstantItem(140).Column(col => {
            //                     col.Item().Text("Datos del Vendedor").Underline().Bold().FontSize(10);
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Nombre: ").SemiBold().FontSize(10);
            //                         txt.Span("Jhovanny Rivera").FontSize(10);
            //                     });
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Telefono: ").SemiBold().FontSize(10);
            //                         txt.Span("829-643-16354").FontSize(10);
            //                     });
            //                     col.Item().Text(txt =>
            //                     {
            //                         txt.Span("Direccion: ").SemiBold().FontSize(10);
            //                         txt.Span("N/A").FontSize(10);
            //                     });
            //                 });

            //             });
            //             col.Item().PaddingVertical(15);
            //             col.Item().AlignRight().Text("Fecha: " + DateTime.Now.ToString("dd/mm/yyyy")).FontSize(12);
            //             //linea
            //             col.Item().PaddingVertical(30).LineHorizontal(0.5f);

            //             col.Item().Table(tabla =>
            //             {
            //                 tabla.ColumnsDefinition(columns =>
            //                 {
            //                     columns.RelativeColumn(3);
            //                     columns.RelativeColumn();
            //                     columns.RelativeColumn();
            //                     columns.RelativeColumn();
            //                 });

            //                 tabla.Header(heather => {

            //                     heather.Cell().Background("#257272").
            //                          Padding(2).Text("Producto").FontSize(14).FontColor("#fff").Bold();

            //                     heather.Cell().Background("#257272").
            //                          Padding(2).Text("Precio Unid").FontSize(14).FontColor("#fff").Bold();

            //                     heather.Cell().Background("#257272").
            //                          Padding(2).Text("Cantidad").FontSize(14).FontColor("#fff").Bold();

            //                     heather.Cell().Background("#257272").
            //                          Padding(2).Text("Total").FontSize(14).FontColor("#fff").Bold();
            //                 });

            //                 foreach (var item in (1,10))
            //                 {
            //                     var cantidad = Placeholders.Random.Next(1, 10);

            //                     var precio = Placeholders.Random.Next(5, 15);

            //                     var total = cantidad * precio;


            //                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(Placeholders.Label()).FontSize(10);
            //                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(cantidad.ToString()).FontSize(10);
            //                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"$/{precio}").FontSize(10);
            //                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"$/{total}").FontSize(10);

            //                 }

            //             });

            //             col.Item().AlignRight().Text("Total:1500").FontSize(12);
            //             col.Item().PaddingVertical(3);
            //             col.Item().Background(Colors.Grey.Lighten2)
            //             .Padding(10).Column(column =>
            //             {
            //                 column.Item().Text("Comentario").FontSize(14);
            //                 column.Item().Text("No aceptamos devoluciones, rebice su compra y/o documentos antes de irse").FontSize(10);
            //                 column.Spacing(10);

            //             });


            //         });
            //         page.Footer()
            //             .Height(75)
            //             .AlignRight()
            //             .AlignMiddle()
            //             .Text(txt =>
            //             {

            //                 txt.Span("Pagina ").FontSize(10);
            //                 txt.CurrentPageNumber().FontSize(10);
            //                 txt.Span(" de ").FontSize(10);
            //                 txt.TotalPages().FontSize(10);
            //             });
            //     });
            // })

            // // instead of the standard way of generating a PDF file
            //.GeneratePdf();

            Stream stream = new MemoryStream();//data

            return File(stream,"application/pdf","detalleventa.pdf");
       }


    }
}