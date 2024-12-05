using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Get all employees
        [HttpGet]

        public async Task<ActionResult<List<EmployeeResponseDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                if (employees == null || employees.Count == 0)
                    return NotFound("No employees found.");

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");


            }
            //This is just example for global exception 
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        [Authorize(Roles = "Admin,Employee")]
        // Get employee's company sales by employee ID
        [HttpGet("companyname/{employeeId}")]
        public async Task<ActionResult<EmployeeCompanyResponseDto>> GetEmployeeCompanySales(int employeeId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeCompanySalesAsync(employeeId);

                if (result == null || (result.TotalSales == 0 && string.IsNullOrEmpty(result.CompanyName)))
                    return NotFound($"No company sales data found for employee with ID {employeeId}.");

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Employee with ID {employeeId} not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}