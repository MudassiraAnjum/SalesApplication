using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;

namespace SalesApplication.IServices.Services
{
    public class EmployeeService:IEmployeeService
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

            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

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
                throw new KeyNotFoundException($"No sales found for date '{date}'");
            }

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                if (sales.EmployeeId != employeeId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this data.");
                }
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


        public async Task<ResponseEmployeeDto> GetHighestSaleByEmployeeAsync(DateTime date)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

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
                .OrderByDescending(x => x.TotalSale)
                .FirstOrDefaultAsync();

            if (sales == null)
            {
                throw new KeyNotFoundException($"No sales found for date '{date}'");
            }

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                if (sales.EmployeeId != employeeId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this data.");
                }
            }

            var employee = await _context.Employees.FindAsync(sales.EmployeeId);
            return _mapper.Map<ResponseEmployeeDto>(employee);
        }

        public async Task<ResponseEmployeeDto> GetHighestSaleByEmployeeAsync(int year)
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

            var highestSale = employeeSales.OrderByDescending(s => s.TotalSale).First();
            var highestEmployee = await _context.Employees.FindAsync(highestSale.EmployeeId);

            return _mapper.Map<ResponseEmployeeDto>(highestEmployee);
        }


        public async Task<ResponseEmployeeDto> GetLowestSaleByEmployeeAsync(DateTime date)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

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
                throw new KeyNotFoundException($"No sales found for date '{date}'");
            }

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                if (sales.EmployeeId != employeeId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this data.");
                }
            }

            var employee = await _context.Employees.FindAsync(sales.EmployeeId);
            return _mapper.Map<ResponseEmployeeDto>(employee);
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



        public async Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<ResponseEmployeeDto> patch)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                if (empid != employeeId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update this employee.");
                }
            }

            var pemployee = await _context.Employees.FindAsync(empid);
            if (pemployee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            var upemployee = _mapper.Map<ResponseEmployeeDto>(pemployee);
            patch.ApplyTo(upemployee);
            _mapper.Map(upemployee, pemployee);
            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseEmployeeDto>(pemployee);
        }

        public async Task<List<EmployeeSalesDto>> GetSalesMadeByEmployeeBetweenDatesAsync(DateTime fromDate, DateTime toDate)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                return await _context.Orders
                    .Include(o => o.ShipViaNavigation)
                    .Where(o => o.EmployeeId == employeeId &&
                                o.OrderDate >= fromDate &&
                                o.OrderDate <= toDate)
                    .Select(o => new EmployeeSalesDto
                    {
                        OrderId = o.OrderId,
                        CompanyName = o.ShipViaNavigation.CompanyName ?? "Unknown Shipper"
                    })
                    .ToListAsync();
            }

            if (userRole == "Admin")
            {
                return await _context.Orders
                    .Include(o => o.ShipViaNavigation)
                    .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                    .Select(o => new EmployeeSalesDto
                    {
                        OrderId = o.OrderId,
                        CompanyName = o.ShipViaNavigation.CompanyName ?? "Unknown Shipper"
                    })
                    .ToListAsync();
            }

            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }

        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                throw new ArgumentNullException(nameof(employeeDto), "Employee data is null.");
            }

            var employee = _mapper.Map<Employee>(employeeDto);

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<(string CompanyName, decimal TotalSales)> GetEmployeeCompanySalesAsync(int employeeId)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var claimedEmployeeId))
            {
                if (claimedEmployeeId != employeeId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this employee's sales data.");
                }
            }
            else if (userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            var result = await _context.Employees
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new
                {
                    TotalSales = e.Orders.Sum(o => o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))),
                    CompanyName = e.Orders.FirstOrDefault().ShipViaNavigation.CompanyName
                })
                .FirstOrDefaultAsync();

            if (result == null)
                throw new KeyNotFoundException("Employee not found.");

            return (result.CompanyName ?? "No company available", result.TotalSales);
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByCityAsync(string city)
        {
            var employees = await _context.Employees
                .Where(e => e.City == city)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByTitleAsync(string title)
        {
            // Find employees with the provided title in their records
            var employees = await _context.Employees
               .Where(e => !string.IsNullOrEmpty(e.Title) && e.Title.Contains(title))
               .ProjectTo<ResponseEmployeeDto>(_mapper.ConfigurationProvider) // AutoMapper Projection
               .ToListAsync();

            return employees;
        }

        public async Task<IEnumerable<EmployeeSalesDto>> SalesMadeByEmployeeDate(int empid, DateTime date)
        {
            var emp = await _context.Orders.Where(e => e.EmployeeId == empid && e.OrderDate.HasValue && e.OrderDate.Value.Date == date.Date).ToListAsync();
            var empdate = emp.Select(o => new EmployeeSalesDto
            {
                OrderId = o.OrderId,
                CompanyName = o.Customer?.CompanyName
            }).ToList();
            return empdate;

        }
    }
}
