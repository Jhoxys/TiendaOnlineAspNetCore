using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Clients
    {
        public int Id { get; set; }

        public Billing Billing { get; set; } = new Billing();


        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";


        public string Email { get; set; } = "";


        public string? PhoneNumber { get; set; }


        public string Address { get; set; } = "";


    }
}
