using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Precision(16, 2)]
        public string Description{ get; set; } = "";

        [Precision(16, 2)]
        public string CodeProduct { get; set; } ="";

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

        public DateTime CreatedAt{ get; set; }

        [Required]
        public decimal Checks { get; set; }

    }
}
