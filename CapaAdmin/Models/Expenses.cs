using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public string Name { get; set; } = "";


        public string Description { get; set; } = "";

        public string CodeProduct { get; set; } = "";

        [Precision(16, 2)]
        public decimal ITB { get; set; }


        [Precision(16, 2)]
        public string NoFactura { get; set; } = "";

        [Precision(16, 2)]
        public decimal Discount { get; set; }

        [Precision(16, 2)]
        public decimal Total { get; set; }

        [Precision(16, 2)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }




    }
}
