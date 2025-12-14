namespace Backend.bangerback.Core.DTOs
{
    public class ResendVerificationDto
    {
        [System.ComponentModel.DataAnnotations.EmailAddress]
        [System.ComponentModel.DataAnnotations.Required]
        public string Email { get; set; } = null!;
    }
}
