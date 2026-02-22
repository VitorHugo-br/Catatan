using System.ComponentModel.DataAnnotations;

namespace MyBlazorApp.Models
{
    public class LoginFormModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
