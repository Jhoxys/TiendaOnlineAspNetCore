using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "The Current pasword field is required")]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "The New pasword field is required")]
        public string NewPassword { get; set; } = "";


        [Required(ErrorMessage = "The Format of the password is not valid")]
        [Compare("NewPassword", ErrorMessage = "Confirm Password and password do not match")]
        public string ConfirmPassword { get; set; } = "";

    }
}
