using System.ComponentModel.DataAnnotations;

namespace SalesApplication.CustomValidation
{
    public class ValidHireDateAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime hireDate)
            {
                if (hireDate > DateTime.Now)
                {
                    return new ValidationResult("Hire date cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
