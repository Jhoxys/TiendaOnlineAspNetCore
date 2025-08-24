using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Typing
    {
        public int Id { get; set; }
  

        public int Quantity { get; set; }


        [Precision(16, 2)]
        public string Name { get; set; } = "";

        [Precision(16, 2)]
        public string Description { get; set; } = "";


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

        public List<Billing> Billing { get; set; } = new List<Billing>();


    }
}
