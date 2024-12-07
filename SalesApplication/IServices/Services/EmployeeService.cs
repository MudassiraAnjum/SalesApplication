using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dtos;
using System.Security.Claims;

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

        // by jaya shree: Get Employees By Region Description
        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByRegionDescAsync(string regionDescription)
        {
            var region = await _context.Regions
                .FirstOrDefaultAsync(r => r.RegionDescription == regionDescription);

            if (region == null)
            {
                throw new KeyNotFoundException($"Region '{regionDescription}' not found.");
            }

            var employees = await _context.Employees
                .Where(e => e.Territories.Any(t => t.RegionId == region.RegionId))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        // by jaya shree: Get Employees By Hire Date
        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByHireDateAsync(DateTime hireDate)
        {
            var employees = await _context.Employees
                .Where(e => EF.Functions.DateDiffDay(e.HireDate, hireDate) == 0)
                .ToListAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException($"Employees hired on '{hireDate}' not found.");
            }

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        // by jaya shree: Get Lowest Sale By Employee On Date
        public async Task<ResponseEmployeeDto> GetLowestSaleByEmpOnDateAsync(DateTime date)
        {
            var sales = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Date == date.Date)
                .GroupBy(od => od.Order.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalSale = g.Sum(od =>
                        od.UnitPrice *
                        (decimal)od.Quantity *
                        (1 - (decimal)od.Discount))
                })
                .OrderBy(x => x.TotalSale)
                .FirstOrDefaultAsync();

            if (sales == null)
            {
                throw new KeyNotFoundException($"No sales found for date '{date}'.");
            }

            var employee = await _context.Employees.FindAsync(sales.EmployeeId);
            return _mapper.Map<ResponseEmployeeDto>(employee);
        }

        // by jaya shree: Get Employees By Date
        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByDateAsync(DateTime date)
        {
            var employees = await _context.Employees
                .Where(e => EF.Functions.DateDiffDay(e.BirthDate, date) == 0)
                .ToListAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException($"No employees found for date '{date}'.");
            }

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        // by jaya shree: Get All Employees
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