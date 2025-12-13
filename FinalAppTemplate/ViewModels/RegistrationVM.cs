using FinalAppTemplate.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FinalAppTemplate.ViewModels
{
    public class RegistrationVM
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(4, ErrorMessage = "Username must be at least 4 chars.")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [EmailChecker]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(7, ErrorMessage = "Password must be at least 7 chars.")]
        [ContainsNumber] // Your custom attribute
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!; // Plain text password
    }
}
