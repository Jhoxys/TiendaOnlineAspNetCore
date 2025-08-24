using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class BillingDto
    {
       // [Required(ErrorMessage = "The Last Name field is required"), MaxLength(100)
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";

        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = "";

        [Required]
        public string CodeProduct { get; set; } = "";

        [Required]
        public decimal ITB { get; set; } 


        [Required]
        public string NoFactura { get; set; } = "";

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public int Quantitys { get; set; }

        [Required]
        public int amount { get; set; }

        [Required]
        public decimal Total { get; set; }


        [Required]
        public decimal Checks { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int TypingId { get; set; }


    }
}
