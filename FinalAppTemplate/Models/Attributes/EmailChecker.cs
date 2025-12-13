using System.ComponentModel.DataAnnotations;


namespace FinalAppTemplate.Models.Attributes
{
    public class EmailChecker : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;

            // 1. Basic Null Check (Let [Required] handle nulls if you use it alongside this)
            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success; 
            }

            // 2. The Logic: Must have '@' and a '.' specifically AFTER the '@'
            // and must NOT end with a dot.
            int atIndex = email.IndexOf('@');
            int lastDotIndex = email.LastIndexOf('.');

            bool hasAt = atIndex > 0;                 // '@' is not the first char
            bool hasDotAfterAt = lastDotIndex > atIndex + 1; // '.' is after '@' (and not immediately after)
            bool doesNotEndWithDot = !email.EndsWith(".");   // e.g. "test@gmail." is INVALID

            if (hasAt && hasDotAfterAt && doesNotEndWithDot)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid email format. It looks like you missed the domain (e.g., .com).");
        }
    }
}
