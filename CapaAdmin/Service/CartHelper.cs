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



    }
}
