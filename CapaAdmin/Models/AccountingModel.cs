using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Models
{
    public class AccountingModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Precision(16, 2)]
        public decimal UnitPrice { get; set; }


        public Product Product { get; set; } = new Product();

    }
}
