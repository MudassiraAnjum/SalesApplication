using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesApplication.IServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDto> DeleteEmployee(int id)
        {
            // Find the employee by id
            var deleteEmp = await _context.Employees.FindAsync(id);

            if (deleteEmp == null)
            {
                throw new KeyNotFoundException("Employee not found. Please check the provided Employee ID.");
            }

            // Remove the employee from the context
            _context.Employees.Remove(deleteEmp);

            // Save changes to the database
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeDto>(deleteEmp);
        }

        public async Task<ResponseEmployeeDto> GetLowestSaleByEmployeeAsync(int year)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            if (!int.TryParse(employeeIdClaim, out var loggedInEmployeeId))
            {
                loggedInEmployeeId = 0;
            }

            var employeeSales = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Year == year)
                .GroupBy(od => od.Order.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalSale = g.Sum(od =>
                        od.UnitPrice *
                        (decimal)od.Quantity *
                        (1 - (decimal)od.Discount))
                })
                .ToListAsync();

            if (employeeSales == null || !employeeSales.Any())
            {
                throw new KeyNotFoundException($"No sales found for the year '{year}'.");
            }

            if (userRole == "Employee")
            {

                var loggedInEmployeeSales = employeeSales.FirstOrDefault(s => s.EmployeeId == loggedInEmployeeId);

                if (loggedInEmployeeSales == null)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this data.");
                }

                var loggedInEmployee = await _context.Employees.FindAsync(loggedInEmployeeId);
                return _mapper.Map<ResponseEmployeeDto>(loggedInEmployee);
            }

            var highestSale = employeeSales.OrderBy(s => s.TotalSale).First();
            var highestEmployee = await _context.Employees.FindAsync(highestSale.EmployeeId);

            return _mapper.Map<ResponseEmployeeDto>(highestEmployee);
        }

        public async Task<List<EmployeeSalesDto>> GetSalesMadeByEmployeeBetweenDatesAsync(int employeeId, DateTime fromDate, DateTime toDate)
        {
            var salesData = await _context.Orders
                .Include(o => o.ShipViaNavigation) // Include Shipper details to get CompanyName
                .Where(o => o.EmployeeId == employeeId &&
                            o.OrderDate >= fromDate &&
                            o.OrderDate <= toDate)
                .Select(o => new EmployeeSalesDto
                {
                    OrderId = o.OrderId,
                    CompanyName = o.ShipViaNavigation.CompanyName ?? "Unknown Shipper"
                })
                .ToListAsync();

            return salesData;
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync()
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee != null)
                {
                    return new List<ResponseEmployeeDto> { _mapper.Map<ResponseEmployeeDto>(employee) };
                }
                throw new KeyNotFoundException($"Employee with ID: {employeeId} not found.");
            }
            if (userRole == "Admin")
            {
                var employees = await _context.Employees.ToListAsync();
                return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
            }

            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }
    }
}

