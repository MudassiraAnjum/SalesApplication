using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesApplication.IServices;
using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Models;
using Microsoft.AspNetCore.Mvc.Core;

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

        // Get employees by city
        public async Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByCityAsync(string city)
        {
            var employees = await _context.Employees
                .Where(e => e.City == city)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        //Get highest sale by employee in a specific year

        public async Task<ResponseEmployeeDto> GetHighestSaleByEmployeeInYearAsync(int year)
        {
            var sales = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Year == year)
                .GroupBy(od => od.Order.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalSale = g.Sum(od => ((decimal)(od.UnitPrice) * (decimal)(od.Quantity)) * (decimal)(1 - (od.Discount)))
                })
                .OrderByDescending(x => x.TotalSale)
                .FirstOrDefaultAsync();

            if (sales == null) return null;

            var employee = await _context.Employees.FindAsync(sales.EmployeeId);
            return _mapper.Map<ResponseEmployeeDto>(employee);
        }
        public async Task<ResponseEmployeeDto> PatchEmployeeAsync(int employeeId, JsonPatchDocument<EmployeeDto> patchDoc)
        {
            // Fetch the employee from the database
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) return null;

            // Map the employee to a DTO to apply the patch
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            // Apply the patch
            patchDoc.ApplyTo(employeeDto);

            // Map the patched DTO back to the entity
            _mapper.Map(employeeDto, employee);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the updated entity to a response DTO
            return _mapper.Map<ResponseEmployeeDto>(employee);
        }


    }

}
