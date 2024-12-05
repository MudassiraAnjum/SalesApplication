using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.Models;


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
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] CreateEmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            var updatedEmployee = await _employeeService.UpdateEmployee(employeeId, employeeDto);
            if (updatedEmployee == null)
            {
                return NotFound();
            }
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPatch("{employeeid}")]
        public async Task<IActionResult> PatchEmployee(int employeeid, JsonPatchDocument<CreateEmployeeDto> patch)
        {
            if (patch == null)
                return BadRequest("Patch data is null");
            var patchemployee = await _employeeService.PatchUpdateEmployee(employeeid, patch);
            if (patchemployee == null)
                return NotFound();
            return Ok(patchemployee);
        }

        [Authorize(Roles = "Admin,Employee")]
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
