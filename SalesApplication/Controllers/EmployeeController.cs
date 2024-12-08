﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
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
        [HttpGet("by-region/{regionDescription}")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetEmployeesByRegion(string regionDescription)
        {
            var employees = await _employeeService.GetEmployeesByRegionDescAsync(regionDescription);
            return Ok(employees);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("by-hire-date/{hireDate}")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetEmployeesByHireDate(DateTime hireDate)
        {
            var employees = await _employeeService.GetEmployeesByHireDateAsync(hireDate);
            return Ok(employees);
        }

        [Authorize(Roles = "Admin,Employee")]
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

        [Authorize(Roles = "Admin,Employee")]
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

        [Authorize(Roles = "Admin,Employee")]

        [HttpGet("All-Employees")]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }


        [HttpGet("highest-sale/by-date")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetHighestSaleByEmployeeAsync([FromQuery] DateTime date)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;

            var result = await _employeeService.GetHighestSaleByEmployeeAsync(date);
            return Ok(result);
        }

        [HttpGet("highest-sale/by-year")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetHighestSaleByEmployeeAsync([FromQuery] int year)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;

            var result = await _employeeService.GetHighestSaleByEmployeeAsync(year);
            return Ok(result);
        }

        [HttpGet("lowest-sale/by-date")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetLowestSaleByEmployeeAsync([FromQuery] DateTime date)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;

            var result = await _employeeService.GetLowestSaleByEmployeeAsync(date);
            return Ok(result);
        }

        [HttpGet("lowest-sale/by-year")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetLowestSaleByEmployeeAsync([FromQuery] int year)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;

            var result = await _employeeService.GetLowestSaleByEmployeeAsync(year);
            return Ok(result);
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

        [HttpGet("sales")]

        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<List<EmployeeSalesDto>>> GetSalesMadeByEmployeeBetweenDatesAsync(DateTime fromDate, DateTime toDate)
        {
            var salesData = await _employeeService.GetSalesMadeByEmployeeBetweenDatesAsync(fromDate, toDate);
            return Ok(salesData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            var result = await _employeeService.AddEmployeeAsync(employeeDto);
            return Ok(result);
        }

        [HttpGet("{employeeId}/sales")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetEmployeeCompanySales(int employeeId)
        {
            var result = await _employeeService.GetEmployeeCompanySalesAsync(employeeId);

            if (result.CompanyName == null)
            {
                return NotFound("Employee not found.");
            }

            return Ok(new { CompanyName = result.CompanyName, TotalSales = result.TotalSales });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("bycity/{city}")]
        public async Task<IActionResult> GetEmployeesByCity(string city)
        {
            var employees = await _employeeService.GetEmployeesByCityAsync(city);
            return Ok(employees);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("title/{title}")]

        public async Task<IActionResult> GetEmployeesByTitle(string title)
        {
            var employees = await _employeeService.GetEmployeesByTitleAsync(title);
            return Ok(employees);
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