using CapaAdmin.Models;
using System.Text.Json;

namespace CapaAdmin.Service
{
    public class CartHelper
    {
        public static Dictionary<int, int> GetCartDiccionary(HttpRequest request, HttpResponse response)
        {
            string cookieValue = request.Cookies["shoppin_cart"] ?? "";

            try
            {
                var cart = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookieValue));

                Console.WriteLine("[CartHelper] cart=" + cookieValue + " -> " + cart);

                var dicionarys= JsonSerializer.Deserialize<Dictionary<int, int>>(cart);

                if (dicionarys != null) 
                    { 
                
                      return dicionarys;
                }

            }catch (Exception) { 
            
            }

            if (cookieValue.Length > 0) { 
               response.Cookies.Delete(cookieValue);// borramos el cookie null porque si no esiste creara uno vacio
            }

             return new Dictionary<int, int>(); 
        }

       public static int GetCartSize(HttpRequest request, HttpResponse response)
        {
            int cartSize = 0;

                  var cartDicionary= GetCartDiccionary(request, response);
            foreach ( var KeyValue in cartDicionary)
            {
                  cartSize += KeyValue.Value;   

            }



            return cartSize;

        }

        public static List<OrderItem> GetCartItems(HttpRequest request, HttpResponse response, ApplicationDbContext context)
        {
           var cartItems = new List<OrderItem>();
            var cartDicionary = GetCartDiccionary(request, response);

            foreach (var Keys in cartDicionary) {
                int productId = Keys.Key;
                int quiantity= Keys.Value;
                var product = context.Products.Find(productId);// desde la base de datos obtener el producto

                if (product == null)  continue;
                   
               var item = new OrderItem
               {
                 Quantity = quiantity,
                 UnitPrice = product.Price,
                 Product=  product,

               };

                cartItems.Add(item);

            }

            return cartItems;
        }
        public static decimal GetSubtotal(List<OrderItem> Cartitems)
        {
            decimal subtotal = 0;

            foreach (var CItem in Cartitems)
            {

                subtotal += CItem.Quantity * CItem.UnitPrice;


            }

            return 2540;
        }

    }
}
