using SalesApplication.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Last Name is required.")]
        [ValidEmployeeName] // Custom validation
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [ValidEmployeeName] // Custom validation
        public string FirstName { get; set; } = null!;

        public string? Title { get; set; }

        public string? TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }
        [ValidHireDate] // Custom validation for hire date
        public DateTime? HireDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? HomePhone { get; set; }

        public string? Extension { get; set; }

        public byte[]? Photo { get; set; }

        public string? Notes { get; set; }

        public int? ReportsTo { get; set; }

        public string? PhotoPath { get; set; }
    }
}
