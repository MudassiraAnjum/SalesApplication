using System.ComponentModel.DataAnnotations;

namespace SalesApplication.Dto
{
    public class OrdersByEmployeeDto
    {
        //[Required(ErrorMessage = "Employee name is required.")]
        //[StringLength(100, ErrorMessage = "Employee name cannot exceed 100 characters.")]
        public string EmployeeName { get; set; }
        public int TotalOrders { get; set; }
    }
}
