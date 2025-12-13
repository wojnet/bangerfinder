using System.ComponentModel.DataAnnotations;

namespace FinalAppTemplate.ViewModels
{
    public class ResendVerificationVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
