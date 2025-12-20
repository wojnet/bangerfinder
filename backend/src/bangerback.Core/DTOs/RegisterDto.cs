using System.ComponentModel.DataAnnotations;

namespace Backend.bangerback.Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(4)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        // You can keep your custom [ContainsNumber] attributes here if you move them to Core
        public string Password { get; set; } = null!;
    }
}
