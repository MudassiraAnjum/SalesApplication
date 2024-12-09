//namespace SalesApplication.Dto
//{
//    public class EmployeeDto
//    {

//        public string? LastName { get; set; }

//        public string? FirstName { get; set; }

//        public string? Title { get; set; }

//        public string? TitleOfCourtesy { get; set; }

//        public DateTime? BirthDate { get; set; }

//        public DateTime? HireDate { get; set; }

//        public string? Address { get; set; }

//        public string? City { get; set; }

//        public string? Region { get; set; }

//        public string? PostalCode { get; set; }

//        public string? Country { get; set; }

//        public string? HomePhone { get; set; }

//        public string? Extension { get; set; }

//        public byte[]? Photo { get; set; }

//        public string? Notes { get; set; }

//        public int? ReportsTo { get; set; }

//        public string? PhotoPath { get; set; }

//    }
//}
using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [MaxLength(50, ErrorMessage = "Title of courtesy cannot exceed 50 characters.")]
        public string? TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; }

        [MaxLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string? Region { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [MaxLength(10, ErrorMessage = "Postal code cannot exceed 10 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? HomePhone { get; set; }

        [MaxLength(10, ErrorMessage = "Extension cannot exceed 10 characters.")]
        public string? Extension { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        public byte[] Photo { get; set; } // Base64 or URL format

        public string? Notes { get; set; } // Allow multiple notes

        public int? ReportsTo { get; set; }
    }
}
