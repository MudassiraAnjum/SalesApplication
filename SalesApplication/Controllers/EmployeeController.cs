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

        public EmployeeController(IEmployeeService _employeeService)
        {
            employeeService = _employeeService;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleteEmp = await employeeService.DeleteEmployee(id);
            return Ok(deleteEmp);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("lowestsalebyemployee/{year}")]
        public async Task<IActionResult> GetLowestSaleByEmpOnYearAsync(int year)
        {
            var getByYear= await employeeService.GetLowestSaleByEmployeeAsync(year);
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
    }
}
