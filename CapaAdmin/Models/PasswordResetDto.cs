using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class PasswordResetDto
    {

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string Password { get; set; } = "";


        [Required(ErrorMessage = "The Format of the Password is not valid")]
        [Compare("Password", ErrorMessage = "Confirm Password and password do not match")]
        public string ConfirmPassword { get; set; } = "";




    }
}
