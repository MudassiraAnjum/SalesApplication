using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;

namespace SalesApplication.IServices.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ResponseOrderDto>> GetAllOrdersAsync()
        {
            
                var orders = await _dbContext.Orders.ToListAsync();
                return _mapper.Map<IEnumerable<ResponseOrderDto>>(orders);
        }
        public async Task<IEnumerable<ShipDetailsDto>> GetAllShipDetailsAsync()
        {
            var orders = await _dbContext.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<ShipDetailsDto>>(orders);
        }
    }
}
