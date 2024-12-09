using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.IServices.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using SalesApplication.Data;

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
        public async Task<IEnumerable<ResponseEmployeeDto>> GetHighestSaleByEmployeeAsync(DateTime date)
        {
            // Find employees who made orders on the specified date
            var employees = await _context.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Date == date.Date)
                .Select(o => o.EmployeeId)
                .Distinct()
                .ToListAsync();

            var employeeList = await _context.Employees
                .Where(e => employees.Contains(e.EmployeeId))
                .ProjectTo<ResponseEmployeeDto>(_mapper.ConfigurationProvider) // AutoMapper Projection
                .ToListAsync();

            return employeeList;
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetHighestSaleByEmployeeAsync(int year)
        {
            // Calculate total sales grouped by EmployeeId
            var highestSales = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Year == year)
                .GroupBy(od => od.Order.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalSales = g.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync();

            // Get the highest total sales value
            var maxSales = highestSales.FirstOrDefault()?.TotalSales;

            if (maxSales == null) return Enumerable.Empty<ResponseEmployeeDto>();

            // Get employees whose total sales match the maximum
            var topEmployees = highestSales
                .Where(s => s.TotalSales == maxSales)
                .Select(s => s.EmployeeId)
                .ToList();

            var employees = await _context.Employees
                .Where(e => topEmployees.Contains(e.EmployeeId))
                .ProjectTo<ResponseEmployeeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return employees;
        }


        // Search employees by their title
        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByTitleAsync(string title)
        {
            // Find employees with the provided title in their records
            var employees = await _context.Employees
               .Where(e => !string.IsNullOrEmpty(e.Title) && e.Title.Contains(title))
               .ProjectTo<ResponseEmployeeDto>(_mapper.ConfigurationProvider) // AutoMapper Projection
               .ToListAsync();

            return employees;
        }
        public async Task<List<OrderInfoDto>> GetSalesByEmployeeOnDateAsync(int employeeId, DateTime date)
        {
            // Query orders made by the employee on the given date and map to DTOs using AutoMapper
            var orders = await _context.Orders
                 .Include(o => o.Customer) // Include related data
                 .Where(o => o.EmployeeId == employeeId && o.OrderDate.HasValue && o.OrderDate.Value.Date == date.Date)
                 .ProjectTo<OrderInfoDto>(_mapper.ConfigurationProvider) // AutoMapper Projection
                 .ToListAsync();

            return orders;
        }

    }
}



