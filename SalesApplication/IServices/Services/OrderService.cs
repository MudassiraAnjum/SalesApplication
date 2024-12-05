using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.Data;
using SalesApplication.IServices;
//using Sales_Application.Models;

namespace Sales_Application.IServices
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

        public async Task<List<ResponseOrderDto>> GetOrderShipperDetailsBetweenDates(DateTime fromDate, DateTime toDate)
        {
            // Query the Orders to get the shipment details within the date range
            var orders = await _context.Orders
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToListAsync();

            // Extract the shipper details from the Orders
            var shipperDetails = orders.Select(o => new ResponseOrderDto
            {
                ShipName = o.ShipName,
                ShipAddress = o.ShipAddress,
                ShipRegion = o.ShipRegion
            }).Distinct().ToList();

            // Return the mapped result
            return shipperDetails;
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




    }
}


