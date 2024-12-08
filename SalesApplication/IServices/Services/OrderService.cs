using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using System.Security.Claims;

namespace SalesApplication.IServices.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetOrder()
        {

            var order = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<ResponseOrderDto>>(order);


        }

        public async Task<IEnumerable<ResponseOrderDto>> GetOrdersByEmpFNameAsync(string firstName)
        {
            // Retrieve the logged-in user's role and employee ID from claims
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            // Check if the user is an Employee
            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var employeeId))
            {
                // Find the employee matching the provided first name
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

                if (employee == null || !employee.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("You are not authorized to access orders for another employee.");
                }

                // Fetch orders for the logged-in employee
                var orders = await _context.Orders
                    .Where(o => o.EmployeeId == employeeId)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ResponseOrderDto>>(orders);
            }
            else if (userRole == "Admin")
            {
                // Admin can access orders for any employee
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));

                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with first name '{firstName}' not found.");
                }

                // Fetch orders for the specified employee
                var orders = await _context.Orders
                    .Where(o => o.EmployeeId == employee.EmployeeId)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ResponseOrderDto>>(orders);
            }

            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
        }
        public async Task<ResponseOrderDto> GetOrderShipperDetailsByOrderId(int orderId)
        {
            // Query the order based on the orderId
            var order = await _context.Orders
                                      .Where(o => o.OrderId == orderId)
                                      .FirstOrDefaultAsync();

            // If the order is not found, return null or handle as needed
            if (order == null)
            {
                return null; // Or you could throw an exception or return a specific status code depending on your business logic
            }

            var orderDto = new ResponseOrderDto
            {
                OrderId = order.OrderId,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipRegion = order.ShipRegion
            };

            return orderDto; // Return the mapped OrderDto
        }
        public async Task<IEnumerable<ShipperDetailsDto>> GetAllShipDetailsAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<ShipperDetailsDto>>(orders);
        }
        public IEnumerable<OrdersShipDetailsDto> GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate)
        {
            var orders = _context.Orders
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToList();

            return _mapper.Map<IEnumerable<OrdersShipDetailsDto>>(orders);
        }
        public IEnumerable<OrdersByEmployeeDto> GetOrdersCountByEmployee()
        {
            // Ensure ShipName and ShipAddress are properly handled as strings (e.g., using .ToString() if they are ntext)
            return _context.Orders
         .GroupBy(o => o.EmployeeId)  // Group by EmployeeId (or EmployeeName, depending on your schema)
         .Select(group => new OrdersByEmployeeDto
         {
             // Assuming Employee has FirstName and LastName properties
             EmployeeName = group.FirstOrDefault().Employee.FirstName + " " + group.FirstOrDefault().Employee.LastName,
             TotalOrders = group.Count()
         })
         .ToList();

        }
    }
}
