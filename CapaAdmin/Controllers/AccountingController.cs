using CapaAdmin.Models;
using CapaAdmin.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using sib_api_v3_sdk.Client;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Document = QuestPDF.Fluent.Document;
using QuestDocument = QuestPDF.Fluent.Document;

namespace CapaAdmin.Controllers
{
    public class AccountingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;
        private readonly int pageSize = 5;
                private readonly string siteName;   
            private readonly string phone;
            private readonly string Country;
            private readonly string State;
            private readonly string Street;
            private readonly string City;
            private readonly string ITBIS;
            private readonly decimal ITBISCalculo;
            private readonly string Email;
             private readonly string Address;


        public AccountingController(UserManager<ApplicationUser> userManager,
			   SignInManager<ApplicationUser> signInManager,ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.configuration = configuration;
            this.env = env;
             siteName = configuration.GetValue<string>("ContactSettings:SiteName") ?? "";// pues solo consigo el telefono 
             phone = configuration.GetValue<string>("ContactSettings:Phone") ?? "";// pues solo consigo el telefono 
             Country = configuration.GetValue<string>("ContactSettings:Country") ?? "";
             State = configuration.GetValue<string>("ContactSettings:State") ?? "";
             Street = configuration.GetValue<string>("ContactSettings:Street") ?? "";
             City = configuration.GetValue<string>("ContactSettings:City") ?? "";
             ITBIS = configuration.GetValue<string>("ITBISSettings:ITBIS") ?? "";
             ITBISCalculo = configuration.GetValue<decimal>("ITBISSettings:ITBISCalculo") ;
            Email = configuration.GetValue<string>("ContactSettings:Email") ?? "";
            Address = configuration.GetValue<string>("ContactSettings:Address") ?? "";
        }

        public async Task<IActionResult> Index()
        {
            //   decimal contar = 0;
            DateTime fechas = DateTime.Now;

            var fecha = DateTime.ParseExact(fechas.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine("Fecha: " + fecha);
            //  var query= context.Inventory.OrderByDescending(p => p.Id).Take(4).ToList();
            var query = await context.Billing.ToListAsync();

            DateTime fechahoy = DateTime.Now;
            decimal quantity = 0;
       



            //  fechahoy = item.CreatedAt.AddDays(15);  esta parte agraga la ganancia de hoy

            var Result = query.Where(o => o.CreatedAt.ToString("dd/MM/yyyy").Equals(fecha.ToString("dd/MM/yyyy")) && o.Debt != 2 && o.Debt != 1);

            if (Result.Count() > 0)
            {
                foreach (var item in Result)
                {
                    quantity += item.Total;

                 
                }

            }
          // else quantity += quantity;


            var week = query.Where(o => o.CreatedAt.DayOfWeek == fecha.DayOfWeek && o.Debt != 2 && o.Debt != 1);
            decimal semana =0;
            semana = (decimal)query
                        .Where(v => v. CreatedAt.DayOfWeek >= DayOfWeek.Monday &&
                                    v.CreatedAt.DayOfWeek <= DayOfWeek.Saturday && v.Debt != 2 && v.Debt != 1 && v.CreatedAt.Month == fecha.Month)
                        .Sum(v => v.Total);


    

            //  ganancias diarias / DailyEarnings
            var month = query.Where(o => o.CreatedAt.Month == fecha.Month && o.Debt != 2 && o.Debt != 1);
            decimal months = 0;
            if (month.Count() > 0)
            {
                foreach (var item in month)
                {
                   months += item.Total;
                }
            }
            
       

         //  var hoys = "hoy.Sum(o => o.DailyEarnings)";
            ViewBag.Today = fecha.ToString("dd/MM/yyyy");
            ViewBag.Month = months;
           // ViewBag.Years = years;

        ViewBag.Weeks = semana;
            ViewBag.Quantity = quantity;
            return View(query);
        }

        [HttpGet]
        public IActionResult Billing(int PageIndex, string? search)
        {

            IQueryable<Product> query = context.Products;


            var random = new Random();
            var numero = random.Next(1000, 9999); // 4 dígitos aleatorios
            string fecha = DateTime.Now.ToString("yyMds"); // Fecha y hora
            string fechaActual = DateTime.Now.ToString("g"); // Fecha y hora
            var fa = $"{numero}-{fecha}";

     
            ViewBag.Phone = phone;
            ViewBag.Country = Country;
            ViewBag.City = City;
            ViewBag.State = State;
            ViewBag.Street = Street;
            ViewBag.ITBIS = ITBIS;
            ViewBag.NoFactura = fa;
            ViewBag.CreaAt = fechaActual;
            ViewBag.ITBISCalculo = ITBISCalculo;
            ViewBag.SiteName = siteName;
            IQueryable<Billing> query2 = context.Billing;

            
          

            // var totalFacturas = Factura.Sum(f => f.Total);
            if (PageIndex < 1)
            {
                PageIndex = 1;
            }
            decimal count2 = query2.Count(); // total del numero de paginas
            int totalPages = (int)Math.Ceiling(count2 / pageSize);
            query2 = query2.Skip((PageIndex - 1) * pageSize).Take(pageSize);

           
            ViewData["PageIndex"] = PageIndex;
            ViewData["TotalPages"] = totalPages;
            var Factura = query2
    .GroupBy(x => x.NoFactura)
    .Select(g => new { 
        
        Totalitys = g.Sum(x => x.Total),
    NoFactura = g.Key,
        NameSeller = g.Select(x => x.NameSeller).FirstOrDefault(),
        Debt = g.Select(x => x.Debt),   
        Checks = g.Sum(x => x.Checks),


        CreatedAt = g.Select(x => x.CreatedAt).FirstOrDefault()

    }

 ).OrderBy(f => f.CreatedAt)
    .ToList();


            ViewBag.Facturas = Factura;
          //  ViewBag.totalFactura = totalFacturas;


            Clients clients = new Clients();

            ViewData["ClientsFirstName"] = clients.FirstName;
            ViewData["Store"] = "Store";
            if (search != null)
            {
                query = query.Where(s => (s.Name.Contains(search) || s.Brand.Contains(search))  && s.Stock > 0);// agregamos una consulta de resultados
                ViewData["activeProduct"] = "block";
                ViewData["Store"] = "Hide Store";
                ViewBag.ProductSeach = query.ToList();
            }
            else
            { ViewData["activeProduct"] = "none";
              ViewData["Store"] = "Store";
            }
            query = query.Where(s =>  s.Stock > 0);// agregamos una consulta de resultados

            ViewBag.ProductSeach = query.ToList();
            ViewData["Search"] = search ?? "";
            return View();
        }
  


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Billing(FacturaDto fact)
        {

            ViewData["PageIndex"] = 1;
            ViewData["TotalPages"] = 1;



            var Factura = context.Billing
              
                .OrderBy(f => f.CreatedAt)
                .ToList().Take(5);

            ViewBag.Facturas = Factura;
            //if (fact == null || fact.BillingDto == null || fact.BillingDto.Count == 0)
            //{
            //    TempData["Error"] = "No se proporcionaron datos de facturación.";
            //    return RedirectToAction("Billing", "Accounting");
            //}   


            ViewBag.DesableArea = "disabled-area";

            decimal totalGeneral = 0;

            string NoFacturaVerific = "";
            int ControlFact = 0;
            decimal ITBValid = 0;
            string ITBIS = configuration.GetValue<string>("ITBISSettings:ITBIS") ?? "";
            decimal ITBISCalculo = configuration.GetValue<decimal>("ITBISSettings:ITBISCalculo");
            if (!ModelState.IsValid)
            {



        foreach(var item in fact.BillingDto)
                {
                    bool existeFactura = context.Billing.Any(o => o.NoFactura == item.NoFactura);

            
               
                   ViewBag.SuccessMessage = "Se envio la factura Total! " + item.Checks;
                    if ( item.Quantitys > 0 && ControlFact >= 1 && !existeFactura  )
                    {
                        var product = context.Products.Find(item.ProductId);

                        if (product != null  &&  Convert.ToInt32(item.CodeProduct) > 0    )
                        {
                            // Descontar stock si existe
                            product.Stock -= item.Quantitys;
                        
                        }
                     


                        // TempData["Error"] = "Se envio la factura sactifactoriamente! " + item.Debt;
                        Billing billings = new Billing()
                    {
                        Description = item.Description ?? "N/A",
                        NoFactura =  item.NoFactura,
                        CodeProduct = item.CodeProduct,
                        Discount = item.Discount,
                        Phone = item.Phone ?? "N/A",
                        Address = item.AddressSeller,
                            NameSeller = item.NameSeller,
                            ITB = item.ITB,
                         Name= item.Name,
                            Price = item.Price,
                        Total = item.Price* item.Quantitys,
                        Debt= fact.DebtType,
                        Quantity = item.Quantitys,
                        Checks = item.Checks,
                        CreatedAt = DateTime.Now      
                    };
                        ViewBag.Facturas = new List<Billing>(); // EMPTY ✔

                        context.Billing.Add(billings); //  inserta

                        if (Convert.ToInt32(item.CodeProduct) <= 0 )
                        {

                            Typing Typ = new Typing()
                            {
                                Name = item.Name,
                                Price = item.Price,
                                Quantity = item.Quantitys,  
                                NoFactura = item.NoFactura,
                                // Discount = item.Discount,
                                Debt = fact.DebtType,
                                Total = item.Price * item.Quantitys,
                                Description = item.Description ?? "N/A",
                                CreatedAt = DateTime.Now


                            };
                            context.Typing.Add(Typ);

                        }
                        NoFacturaVerific += item.NoFactura;

                    }
                    ControlFact ++;
                }        
                // Aquí insertas en BD y descontas stock…

                // vcontext.Bil
                context.SaveChanges();

               // ViewBag.SuccessMessage = "Se envio la factura sactifactoriamente!";
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
                 col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:SiteName")).Bold().FontSize(6);
                 col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:Address")).Bold().FontSize(3);
                 col.Item().AlignCenter().Text("Email: "+ configuration.GetValue<string>("ContactSettings:Email")).Bold().FontSize(3);
                 col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:Phone")).Bold().FontSize(3);
                
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
                                               Padding(2).Text("Cant").FontSize(3).FontColor("#fff").Bold();
                                         heather.Cell().Background("#257272").
                                              Padding(2).Text("P/Unid").FontSize(3).FontColor("#fff").Bold();
                                         heather.Cell().Background("#257272").
                                               Padding(2).Text("Total").FontSize(3).FontColor("#fff").Bold();
                                     });
                                  foreach (var item in fact.BillingDto)
                                  {
                                      if (item.Quantitys <= 0  || item.Price <= 0)
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
                                          tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.Description).FontSize(3);
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

               /// Stream stream = new MemoryStream(data);
                // Carpeta donde guardaremos (ej: wwwroot/Pdfs)
    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pdfs");
    if (!Directory.Exists(folderPath))
        Directory.CreateDirectory(folderPath);

    // Nombre único para evitar sobreescrituras
    string fileName = $"Fact-{DateTime.Now:yyyyMMddHHmmss}.pdf";
    string filePath = Path.Combine(folderPath, fileName);

    // Guardar el PDF en el servidor
                await System.IO.File.WriteAllBytesAsync(filePath, data);

                // Construir URL pública
                string pdfUrl = Url.Content($"~/Pdfs/{fileName}");

                // Pasar al ViewBag
                ViewBag.PdfPath = pdfUrl;
                //  return File(stream, "application/pdf", $"Fact-{DateTime.Now}.pdf");
                //  return RedirectToAction("Billing", "Accounting");
                /// + RedirectToAction("Billing", "Accounting")
                /// 
                return View(fact);
            }
           

            return RedirectToAction("Billing", "Accounting");


        }


        public async Task<IActionResult> DescargarPDFFinal2(string fact)
        {
            decimal totalGeneral = 0;
            string NoFacturaVerific = "";
            int ControlFact = 0;
            decimal ITBValid = 0;
            string ITBIS = configuration.GetValue<string>("ITBISSettings:ITBIS") ?? "";
            decimal ITBISCalculo = configuration.GetValue<decimal>("ITBISSettings:ITBISCalculo");

            var query =  await context.Billing.ToListAsync();

            var fillPrincy = query.Where(o => o.NoFactura == fact).FirstOrDefault();

            if (fillPrincy == null)
            {
                TempData["Error"] = "Factura no encontrada.";
                return RedirectToAction("Billing", "Accounting");
            }
             ViewBag.DesableArea = "disabled-area";

     


            // ViewBag.SuccessMessage = "Se envio la factura sactifactoriamente!";
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
                            col.Item().AlignCenter().Height(20).Image(imagePath);
                            col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:SiteName")).Bold().FontSize(6);
                            col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:Address")).Bold().FontSize(3);
                            col.Item().AlignCenter().Text("Email: " + configuration.GetValue<string>("ContactSettings:Email")).Bold().FontSize(3);
                            col.Item().AlignCenter().Text(configuration.GetValue<string>("ContactSettings:Phone")).Bold().FontSize(3);

                        });


                    });



                    page.Content().Column(column =>
                    {
                        column.Item().PaddingVertical(2);
                        column.Item().Text("Factura").FontSize(5).Bold();
                        column.Item().AlignRight().Text("Fecha: " + DateTime.Now.ToString("d")).FontSize(3).Bold();
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
                                      Padding(2).Text("Cant").FontSize(3).FontColor("#fff").Bold();
                                heather.Cell().Background("#257272").
                                     Padding(2).Text("P/Unid").FontSize(3).FontColor("#fff").Bold();
                                heather.Cell().Background("#257272").
                                      Padding(2).Text("Total").FontSize(3).FontColor("#fff").Bold();
                            });
                            foreach (var item in query.Where(t=>t.NoFactura == fact))
                            {
                                if (item.Quantity <= 0 || item.Price <= 0)
                                {

                                }
                                else
                                {
                                    
                                    var cantidad = item.Quantity;
                                    var precio = item.Price;
                                    var total = cantidad * precio;
                                     totalGeneral += total;
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.Description).FontSize(3);
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(cantidad.ToString()).FontSize(3);
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"${precio}").FontSize(3);
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"${total}").FontSize(3);
                                }

                            }
                            var CalITBIS = totalGeneral * ITBISCalculo;
                            var SumaTotalITB = totalGeneral + CalITBIS;
                            column.Item().PaddingVertical(2);
                            column.Item().AlignRight().Text("Sub total:$" + totalGeneral.ToString("N2")).FontSize(3).Bold();
                            if (ITBValid > 0)
                            {
                                column.Item().AlignRight().Text($"ITB({ITBIS}%)$" + CalITBIS).FontSize(3).Bold();
                                column.Item().AlignRight().Text("Total:" + SumaTotalITB.ToString("N2")).FontSize(3).Bold();
                            }
                            else { column.Item().AlignRight().Text("Total:" + totalGeneral.ToString("N2")).FontSize(3).Bold(); }

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

            /// Stream stream = new MemoryStream(data);
            // Carpeta donde guardaremos (ej: wwwroot/Pdfs)
            //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDF-FAC");
            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            //// Nombre único para evitar sobreescrituras
            //string fileName = $"Fact-{DateTime.Now:yyyyMMddHHmmss}.pdf";
            //string filePath = Path.Combine(folderPath, fileName);

            //// Guardar el PDF en el servidor
            //await System.IO.File.WriteAllBytesAsync(filePath, data);

            //// Construir URL pública
           // string pdfUrl = Url.Content($"~/Pdfs/{fileName}");


            return View(fact);
        }




        public async Task <IActionResult> DescargarPDF(FacturaDto fact)
        {
            decimal ITBValid2 = 0;

            // validar la fucking licencia 
            QuestPDF.Settings.License = LicenseType.Community;
            // // validar la fucking licencia 

            QuestPDF.Settings.EnableDebugging = true;

        

            //ViewBag.SuccessMessage = "Se envio la factura Total! ";
            //// TempData["SuccessMessage"] = "Se envió la factura total!";

            IQueryable<Billing> queryable = context.Billing;
            decimal totality = 0;
            var appUser = await userManager.GetUserAsync(User);

            var DatCliente = queryable.Where(a => a.NoFactura == fact.BillingDto.First().NoFactura);
            //// code in your main method
            var data = QuestPDF.Fluent.Document.Create(page =>
                 {
                     page.Page(page =>
                     {
                         page.Size(PageSizes.Letter);
                         page.Margin(2, Unit.Centimetre);
                         page.DefaultTextStyle(x => x.FontSize(24));
                         page.Header().ShowOnce()
                              .Row(row =>
                              {

                                  row.ConstantItem(150).Height(60).Image("C://diseños//Logo proximab copy.jpg");
                                  row.RelativeItem().Column(col =>
                                  {

                                      col.Item().AlignCenter().Text(siteName).Bold().FontSize(18);
                                      col.Item().AlignCenter().Text(Address).Bold().FontSize(9);
                                      col.Item().AlignCenter().Text("Email:"+ Email).Bold().FontSize(9);
                                      col.Item().AlignCenter().Text("Tel:"+ phone).Bold().FontSize(9);


                                  });
                                  row.ConstantItem(105).Column(col =>
                                  {

                                      col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("RNC:0000").Bold().FontSize(11);
                                      col.Item().BorderColor("#257272").Border(1).Background("#257272").AlignCenter().Text("Codigo Venta").Bold().FontSize(11).FontColor("#fff");
                                      col.Item().BorderColor("#257272").Border(1).AlignCenter().Text("No.:"+fact.BillingDto.First().NoFactura).Bold().FontSize(11);


                                  });

                              });
                         page.Content().PaddingVertical(30).Column(col =>
                         {

                             col.Item().Row(rows =>
                             {

                                 rows.ConstantItem(105).Column(col =>
                                 {
                                     col.Item().Text("Datos del cliente").Underline().Bold().FontSize(10);
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Nombre: ").SemiBold().FontSize(10);
                                         txt.Span(DatCliente.First().Name).FontSize(10);
                                     });
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Cedula: ").SemiBold().FontSize(10);
                                         txt.Span(DatCliente.First().Phone ?? "N/A").FontSize(10);
                                     });
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Direccion: ").SemiBold().FontSize(10);
                                         txt.Span(DatCliente.First().Address ?? "N/A").FontSize(10);
                                     });
                                 });
                                 rows.RelativeItem().Column(col =>
                                 {

                                 });
                                 rows.ConstantItem(140).Column(col =>
                                 {
                                     col.Item().Text("Datos del Vendedor").Underline().Bold().FontSize(10);
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Nombre: ").SemiBold().FontSize(10);
                                         txt.Span(appUser.FirstName+" "+ appUser.LastName).FontSize(10);
                                     });
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Telefono: ").SemiBold().FontSize(10);
                                         txt.Span(phone).FontSize(10);
                                     });
                                     col.Item().Text(txt =>
                                     {
                                         txt.Span("Direccion: ").SemiBold().FontSize(10);
                                         txt.Span(Address).FontSize(10);
                                     });
                                 });

                             });
                             col.Item().PaddingVertical(15);
                             col.Item().AlignRight().Text("Fecha: " + DateTime.Now.ToString("dd/mm/yyyy")).FontSize(12);
                             //linea
                             col.Item().PaddingVertical(30).LineHorizontal(0.5f);

                             col.Item().Table(tabla =>
                             {
                                 tabla.ColumnsDefinition(columns =>
                                 {
                                     columns.RelativeColumn(3);
                                     columns.RelativeColumn();
                                     columns.RelativeColumn();
                                     columns.RelativeColumn();
                                 });

                                 tabla.Header(heather =>
                                 {

                                     heather.Cell().Background("#257272").
                                          Padding(2).Text("Producto").FontSize(14).FontColor("#fff").Bold();

                                     heather.Cell().Background("#257272").
                                          Padding(2).Text("Precio Unid").FontSize(14).FontColor("#fff").Bold();

                                     heather.Cell().Background("#257272").
                                          Padding(2).Text("Cantidad").FontSize(14).FontColor("#fff").Bold();

                                     heather.Cell().Background("#257272").
                                          Padding(2).Text("Total").FontSize(14).FontColor("#fff").Bold();
                                 });
                                 var billing = queryable.ToList();

                              


                                 foreach (var item in billing.Where(b => b.NoFactura == fact.BillingDto.First().NoFactura))
                                 {

                                     if (item.Checks > 0)
                                     {
                                         ITBValid2++;
                                     }


                                     var cantidad = item.Quantity;

                                     var precio = item.Price;

                                     var total = item.Total;
                                     var description = item.Description;
                                     totality += total;
                                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(description).FontSize(10);
                                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"$/{precio}").FontSize(10);
                                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(cantidad).FontSize(10);
                                     tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"$/{total}").FontSize(10);

                                 }


                             });

                             var CalITBIS = totality * ITBISCalculo;
                             var SumaTotalITB = totality + CalITBIS;
                             col.Item().PaddingVertical(2);
                             col.Item().AlignRight().Text("Sub total:$" + totality.ToString("N2")).FontSize(10).Bold();
                             if (ITBValid2 > 0)
                             {
                                 col.Item().AlignRight().Text($"ITB({ITBIS}%)$" + CalITBIS).FontSize(10).Bold();
                                 col.Item().AlignRight().Text("Total:" + SumaTotalITB.ToString("N2")).FontSize(10).Bold();
                             }
                             else { col.Item().AlignRight().Text("Total:" + totality.ToString("N2")).FontSize(10).Bold(); }


                             col.Item().PaddingVertical(3);
                             col.Item().Background(Colors.Grey.Lighten2)
                             .Padding(10).Column(column =>
                             {
                                 column.Item().Text("Comentario").FontSize(14);
                                 column.Item().Text("No aceptamos devoluciones, rebice su compra y/o documentos antes de irse").FontSize(10);
                                 column.Spacing(10);

                             });


                         });
                         page.Footer()
                             .Height(75)
                             .AlignRight()
                             .AlignMiddle()
                             .Text(txt =>
                             {

                                 txt.Span("Pagina ").FontSize(10);
                                 txt.CurrentPageNumber().FontSize(10);
                                 txt.Span(" de ").FontSize(10);
                                 txt.TotalPages().FontSize(10);
                             });
                     });
                 })

                // instead of the standard way of generating a PDF file
                .GeneratePdf();

                 Stream stream = new MemoryStream(data);//data


            return File(stream,"application/pdf","detalleventa.pdf");
            // return View();
        }


    }
}