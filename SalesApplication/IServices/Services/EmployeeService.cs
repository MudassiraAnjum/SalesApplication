using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dtos;

namespace SalesApplication.IServices.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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

        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByHireDateAsync(DateTime hireDate)
        {
            var employees = await _context.Employees
                .Where(e => EF.Functions.DateDiffDay(e.HireDate, hireDate) == 0)
            .ToListAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException($"Region '{hireDate}' not found.");
            }

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

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
                throw new KeyNotFoundException($"Region '{date}' not found.");
            }

            var employee = await _context.Employees.FindAsync(sales.EmployeeId);
            return _mapper.Map<ResponseEmployeeDto>(employee);
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByDateAsync(DateTime date)
        {
            var employees = await _context.Employees
                .Where(e => EF.Functions.DateDiffDay(e.BirthDate, date) == 0)
                .ToListAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException($"Region '{date}' not found.");
            }

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

    }
}