using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;
using SalesApplication.Models;
using System.Security.Claims;


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
        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("update/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] CreateEmployeeDto employeeDto)
        {
            // Check if the employeeDto is null
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            // Call the service method to update the employee
            var updatedEmployee = await _employeeService.UpdateEmployeeByAsync(employeeId, employeeDto, HttpContext.User);

            // Check if the update was successful
            if (updatedEmployee == null)
            {
                return NotFound("Employee not found or you do not have permission to update this employee's details.");
            }

            // Return the updated employee details
            return Ok(updatedEmployee);
        }

        [HttpPatch("{empid}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> PatchUpdateEmployee(int empid, [FromBody] JsonPatchDocument<ResponseEmployeeDto> patch)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;

            var result = await _employeeService.PatchUpdateEmployee(empid, patch);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{Salesempid}")]
        public async Task<IActionResult> GetSalesEmployeeMadeOnDate(int Salesempid, DateTime date)
        {
            var saledate = await _employeeService.SalesMadeByEmployeeDate(Salesempid, date);
            if (saledate == null || !saledate.Any())
            {
                return NotFound("No sales found for this employee on the given date.");
            }
            return Ok(saledate);
        }
    }
}
