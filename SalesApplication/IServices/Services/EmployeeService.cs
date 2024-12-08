using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using SalesApplication.IServices;
using System.Security.Claims;

namespace SalesApplication.IServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        public EmployeeService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<ResponseEmployeeDto> UpdateEmployeeByAsync(int employeeId, CreateEmployeeDto employeeDto, ClaimsPrincipal user)
        {
            // Get the current user's EmployeeId from claims
            var currentUserId = user.FindFirst("EmployeeId")?.Value;

            // Check if the user is an employee and is trying to update their own details
            if (currentUserId == null || (currentUserId != employeeId.ToString() && !user.IsInRole("Admin")))
            {
                throw new UnauthorizedAccessException("You do not have permission to update this employee's details.");
            }

            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            // Map the updated details from employeeDto to the employee entity
            _mapper.Map(employeeDto, employee);

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseEmployeeDto>(employee);
        }

        public async Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<ResponseEmployeeDto> patch)
        {
            var userRole = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _contextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

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

        public async Task<IEnumerable<SalesEmployeeDateDto>> SalesMadeByEmployeeDate(int empid, DateTime date)
        {
            var emp = await _context.Orders.Where(e => e.EmployeeId == empid && e.OrderDate.HasValue && e.OrderDate.Value.Date == date.Date).ToListAsync();
            var empdate = emp.Select(o => new SalesEmployeeDateDto
            {
                OrderId = o.OrderId,
                CompanyName = o.Customer?.CompanyName
            }).ToList();
            return empdate;

        }

        //public async Task<IEnumerable<SalesEmployeeDateDto>> SalesMadeByEmployeeDate(int empid, DateTime date)
        //{

        //    var orders = await _context.Orders
        //        .Where(o => o.EmployeeId == empid && o.OrderDate.HasValue && o.OrderDate.Value.Date == date.Date)
        //        .ToListAsync();


        //    var salesDetails = orders.Select(order => new SalesEmployeeDateDto
        //    {
        //        OrderId = order.OrderId,
        //        CompanyName = order.Customer?.CompanyName ?? "Unknown" 
        //    }).ToList();

        //    return salesDetails;
        //}

    }
}