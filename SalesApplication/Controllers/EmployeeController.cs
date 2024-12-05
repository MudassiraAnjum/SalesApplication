using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // Get employees by title
        [Authorize(Roles = "Employee,Admin")]
        [HttpGet("title/{title}")]
        
        public async Task<IActionResult> GetEmployeesByTitle(string title)
        {
            var employees = await _employeeService.GetEmployeesByTitleAsync(title);

            // Map collection of employees to DTOs
            var employeeDtos = _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        // Get employees with the highest sale on a specific date
        [Authorize(Roles = "Employee,Admin")]

        [HttpGet("highestsalebyemployee/{date}")]
        public async Task<IActionResult> GetHighestSaleByEmployeeAsync(DateTime date)
        {
            var employees = await _employeeService.GetHighestSaleByEmployeeAsync(date);

            // Map collection of employees to DTOs
            var employeeDtos = _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        // Get employee with the highest sale in a specific year
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("highestsalebyemployee/{year:int}")]
        public async Task<IActionResult> GetHighestSaleByEmployeeAsync(int year)
        {
            var employees = await _employeeService.GetHighestSaleByEmployeeAsync(year);

            // Map collection of employees to DTOs
            var employeeDtos = _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("salemadebyanemployee/{employeeId}/{date}")]
        public async Task<IActionResult> GetSalesByEmployeeOnDate(int employeeId, DateTime date)
        {
            
                var sales = await _employeeService.GetSalesByEmployeeOnDateAsync(employeeId, date);

                if (sales == null || !sales.Any())
                    return NotFound($"No sales found for Employee ID {employeeId} on {date:yyyy-MM-dd}.");

                // Use AutoMapper to explicitly map results to DTOs
                var mappedSales = _mapper.Map<List<OrderInfoDto>>(sales);

                return Ok(mappedSales);
            
        }
    }
}



