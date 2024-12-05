using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.IServices;

namespace SalesApplication.IServices.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetOrder()
        {

            var order = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<ResponseOrderDto>>(order);


        }
    }
}
