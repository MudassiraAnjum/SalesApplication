using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dtos;

namespace SalesApplication.IServices.Services
{
    public class OrderDetailsService:IOrderDetailsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ResponseOrderDetailsDto>> GetOrderDetailsByEmployeeId(int employeeId)
        {
            var result = await _dbContext.OrderDetails
                .Where(od => od.Order.EmployeeId == employeeId)
                .Select(od => new ResponseOrderDetailsDto
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    // Calculate BillAmount
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
