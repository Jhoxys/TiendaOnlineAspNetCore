using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public int Quantity { get; set; }


        public string Name { get; set; } = "";

     
        public string Description{ get; set; } = "";

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

        [Precision(16, 2)]
        public decimal Checks { get; set; }

        public int? TypingId { get; set; }// para que acepte null



        public List<Inventory> Inventories { get; set; }= new List<Inventory>();


    }
}
