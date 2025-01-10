using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class ChekckoutDto
    {
        [Required(ErrorMessage ="The Delivery Address is Required.")]
        [MaxLength(200)]
        public string DeliveryAddreess { get; set; } =  string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;


    }
}
