using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using System.Security.Claims;

namespace SalesApplication.IServices.Services
{
    public class OrderDetailService:IOrderDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderDetailService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IEnumerable<ResponseOrderDetailsDto>> GetAllOrderDetails()
        {
            var orderDetails = await _context.OrderDetails.Include(o => o.Order).Include(p => p.Product).ToListAsync();
            return _mapper.Map<IEnumerable<ResponseOrderDetailsDto>>(orderDetails);
        }
        public async Task<List<ResponseOrderDetailsDto>> GetOrderDetailsByEmployeeId(int employeeId)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("EmployeeId")?.Value;

            // Check if the user is an employee and if they are trying to access their own order details
            if (userRole == "Employee" && int.TryParse(employeeIdClaim, out var currentEmployeeId))
            {
                if (currentEmployeeId != employeeId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this employee's order details.");
                }
            }
            else if (userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            // If the user is an admin or the employee is accessing their own order details
            var result = await _context.OrderDetails
                .Where(od => od.Order.EmployeeId == employeeId)
                .Select(od => new ResponseOrderDetailsDto
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    BillAmount = od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)
                })
                .ToListAsync();

            if (result == null || !result.Any())
            {
                throw new KeyNotFoundException($"No order details found for EmployeeID: {employeeId}");
            }

            return result;
        }
    }
}
