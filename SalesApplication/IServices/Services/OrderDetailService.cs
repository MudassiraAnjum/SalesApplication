using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;

namespace SalesApplication.IServices.Services
{
    public class OrderDetailService:IOrderDetailService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ResponseOrderDetailDto>> GetAllOrderDetails()
        {
            var orderDetails = await _dbContext.OrderDetails.Include(o => o.Order).Include(p => p.Product).ToListAsync();
            return _mapper.Map<IEnumerable<ResponseOrderDetailDto>>(orderDetails);
        }
    }
}
