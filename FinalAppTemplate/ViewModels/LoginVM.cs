using System.ComponentModel.DataAnnotations;

namespace FinalAppTemplate.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Enter your username or email")]
        [Display(Name = "Username or Email")]
        public string LoginInput { get; set; } = null!; // This holds either value

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
