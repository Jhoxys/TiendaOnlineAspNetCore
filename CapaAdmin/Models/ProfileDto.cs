using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class ProfileDto
    {

        [Required(ErrorMessage = "The First Name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "The Last Name field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";


        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";


        [Required(ErrorMessage = "The Format of the Phone is not valid"), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; } = "";


    }
}
