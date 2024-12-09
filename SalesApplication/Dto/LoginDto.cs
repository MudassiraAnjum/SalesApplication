using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }
    }
}