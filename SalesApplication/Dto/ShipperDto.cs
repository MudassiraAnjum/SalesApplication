using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SalesApplication.Dto
{
    public class ShipperDto
    {
        [Required]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Company name must start with a capital letter.")]
        public string CompanyName { get; set; } = null!;

        [Phone, Required]
        public string? Phone { get; set; }

        [EmailAddress, Required]
        public string? Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; } = null!;
    }
}

