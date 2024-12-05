using System.ComponentModel.DataAnnotations;

namespace SalesApplication.CustomValidation
{
    public class ValidEmployeeNameAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                return new ValidationResult("Name must be at least 2 characters long.");
            }
            return ValidationResult.Success;
        }
    }
}