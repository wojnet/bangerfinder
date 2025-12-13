using System.ComponentModel.DataAnnotations;

namespace FinalAppTemplate.Models.Attributes
{
    public class ContainsNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // 1. If the field is null (empty), we let [Required] handle that check.
            // We only care about checking the content if it actually exists.
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string password = value.ToString()!;

            // 2. The readable logic: "Does any character in this string qualify as a digit?"
            bool hasNumber = password.Any(char.IsDigit);

            if (hasNumber)
            {
                return ValidationResult.Success;
            }
            else
            {
                // Return the error message defined in the model or a default one
                return new ValidationResult(ErrorMessage ?? "The password must contain at least one number.");
            }
        }
    }
}
