using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dtos;
using SalesApplication.IServices;

namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("by-region/{regionDescription}")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetEmployeesByRegion(string regionDescription)
        {
            var employees = await _employeeService.GetEmployeesByRegionDescAsync(regionDescription);
            return Ok(employees);
        }

        [HttpGet("by-hire-date/{hireDate}")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetEmployeesByHireDate(DateTime hireDate)
        {
            var employees = await _employeeService.GetEmployeesByHireDateAsync(hireDate);
            return Ok(employees);
        }

        [HttpGet("lowest-sale/{date}")]
        public async Task<ActionResult<ResponseEmployeeDto>> GetLowestSaleByEmployeeOnDate(DateTime date)
        {
            var result = await _employeeService.GetLowestSaleByEmpOnDateAsync(date);
            if (result == null)
            {
                return NotFound(new { Message = "No sales found for the specified date." });
            }
            return Ok(result);
        }

        [HttpGet("birthday-by-date")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetEmployeesWithBirthdayToday(DateTime date)
        {
            var employees = await _employeeService.GetEmployeesByDateAsync(date);
            if (employees == null || !employees.Any())
            {
                return NotFound("No employees found with a birthday today.");
            }
            return Ok(employees);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
    }
}