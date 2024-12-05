using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
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
        [Authorize(Roles ="Admin,Employee")]
        [HttpGet("bycity/{city}")]
        public async Task<IActionResult> GetEmployeesByCity(string city)
        {
            var employees = await _employeeService.GetEmployeesByCityAsync(city);
            return Ok(employees);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("highest-sale-in-year/{year:int}")]
        public async Task<IActionResult> GetHighestSaleByEmployeeInYear(int year)
        {
            var employee = await _employeeService.GetHighestSaleByEmployeeInYearAsync(year);
            return employee == null ? NotFound($"No sales data found for the year {year}.") : Ok(employee);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PatchEmployee(int employeeId, [FromBody] JsonPatchDocument<EmployeeDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is null.");
            }

            var updatedEmployee = await _employeeService.PatchEmployeeAsync(employeeId, patchDoc);
            if (updatedEmployee == null)
            {
                return NotFound();
            }
            return Ok(updatedEmployee);
        }
    }
}
