using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using SalesApplication.IServices;

namespace SalesApplication.IServices.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderDetailService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseOrderDetailsDto>> GetOrderDetailById(int eid)
        {
            var orderDetails = await _context.Orders
            .Where(o => o.EmployeeId == eid)
            .Select(o => new
            {
                OrderId = o.OrderId,
                BillAmount = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
            })
            .ToListAsync();

            return orderDetails.Select(od => new ResponseOrderDetailsDto
            {
                OrderId = od.OrderId,
                BillAmount = od.BillAmount
            }).ToList();
        }
    }
}