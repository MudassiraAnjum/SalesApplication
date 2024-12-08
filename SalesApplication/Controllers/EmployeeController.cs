using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;

namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        //IEmployeeService i;

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
            //i = new EmployeeService();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleteEmp = await employeeService.DeleteEmployee(id);
            return Ok(deleteEmp);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("lowestsalebyemployee/{year}")]
        public async Task<IActionResult> GetLowestSaleByEmpOnYearAsync(int year)
        {
            var getByYear= await employeeService.GetLowestSaleByEmpInYearAsync(year);
            return Ok(getByYear);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Salemadebyanemployeebetweendates/{EmployeeId}/{fromdate}/{todate}")]
        public async Task<IActionResult> GetSalesMadeByEmployeeBetweenDatesAsync(int EmployeeId, DateTime fromdate, DateTime todate)
        {
                var salesData = await employeeService.GetSalesMadeByEmployeeBetweenDatesAsync(EmployeeId, fromdate, todate);

                if (salesData == null || salesData.Count == 0)
                {
                    return NotFound(new { message = "No sales found for the employee in the given date range." });
                }

                return Ok(salesData);
        }

        //[Authorize(Roles = "Admin,Employee")]
        //[HttpGet("All-Employees")]
        //public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetAllEmployees()
        //{
        //    var employees = await employeeService.GetAllEmployeesAsync();
        //    return Ok(employees);
        //}

    }
}
