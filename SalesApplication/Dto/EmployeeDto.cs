using System;
using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string? Title { get; set; }

        [StringLength(50, ErrorMessage = "Title of courtesy cannot exceed 50 characters.")]
        public string? TitleOfCourtesy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string? Region { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string? Country { get; set; }

        [StringLength(20, ErrorMessage = "Home phone cannot exceed 20 characters.")]
        [Phone(ErrorMessage = "Home phone must be a valid phone number.")]
        public string? HomePhone { get; set; }

        [StringLength(10, ErrorMessage = "Extension cannot exceed 10 characters.")]
        public string? Extension { get; set; }

        public byte[]? Photo { get; set; } // No validation required for Photo

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
        public string? PhotoPath { get; set; }
        public int? ReportsTo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = null!;
    }
}
