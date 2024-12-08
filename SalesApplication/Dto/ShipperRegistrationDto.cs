using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class ShipperRegistrationDto
    {
        public string CompanyName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
