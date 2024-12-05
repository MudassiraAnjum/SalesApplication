using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;

namespace SalesApplication.IServices.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetOrdersByEmployeeAsync(string firstName)
        { 
                // Find the employee based on the FirstName
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.FirstName == firstName);

                // If employee is not found, return a specific message
                if (employee == null)
                {
                    throw new Exception($"Employee with First Name '{firstName}' not found.");
                }

                // Get all orders placed by the employee
                var orders = await _context.Orders
                    .Where(o => o.EmployeeId == employee.EmployeeId) // Filter orders by EmployeeId
                    .ToListAsync();

                // If no orders are found, return an empty list or an appropriate message
                if (orders == null || orders.Count == 0)
                {
                    throw new Exception($"No orders found for employee with First Name '{firstName}'.");
                }

                // Map the orders to OrderDto using AutoMapper
                var orderDtos = _mapper.Map<List<OrderDto>>(orders);

                return orderDtos;
        }


    }
}
