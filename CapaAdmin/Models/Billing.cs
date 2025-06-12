using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Precision(16, 2)]
        public string Description{ get; set; } = "";

        [Precision(16, 2)]
        public decimal CodeProduct { get; set; }

        [Precision(16, 2)]
        public decimal ITB { get; set; }


        [Precision(16, 2)]
        public string NoFactura { get; set; } = "";

        [Precision(16, 2)]
        public decimal Discount { get; set; }

        [Precision(16, 2)]
        public decimal Total { get; set; }


        public Product Product { get; set; } = new Product();
        public int ProductId { get; set; }

        public DateTime CreatedAt{ get; set; } 

    }
}
