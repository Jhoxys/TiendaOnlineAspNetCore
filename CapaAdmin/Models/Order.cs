using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Order
    {

        public int Id { get; set; }

        public string ClienteId { get; set; } = "";
        public ApplicationUser Client{ get; set; } = null!;

        public List <OrderItem> Items{ get; set; } = new List<OrderItem>();

        [Precision(16,2)]
        public decimal ShippingFree { get; set; }

        public string DeliveryAddres { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public string PaymentDetails { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public DateTime CreatedAt{ get; set; } 
    }
}
