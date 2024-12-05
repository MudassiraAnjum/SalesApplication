using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using SalesApplication.IServices;

namespace SalesApplication.IServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ResponseEmployeeDto> UpdateEmployee(int employeeId, CreateEmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                throw new Exception("Employee not found");


            _mapper.Map(employeeDto, employee);


            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();


            return _mapper.Map<ResponseEmployeeDto>(employee);
        }

        public async Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<CreateEmployeeDto> patch)
        {
            var pemployee = await _context.Employees.FindAsync(empid);
            if (pemployee == null)
                throw new Exception("Employee not found");

            var upemployee = _mapper.Map<CreateEmployeeDto>(pemployee);
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