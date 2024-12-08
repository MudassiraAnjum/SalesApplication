using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.Models;


namespace SalesApplication.IServices.Services
{
    public class ShipperService : IShipperService
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public ShipperService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<List<ShipperTotalAmountDto>> GetTotalAmount()
        {
            try
            {
                var earnings = await _context.Orders
                    .GroupBy(o => o.ShipVia)
                    .Select(g => new ShipperTotalAmountDto
                    {
                        CompanyName = _context.Shippers.FirstOrDefault(s => s.ShipperId == g.Key).CompanyName,
                        TotalAmount = g.Sum(o => o.Freight) ?? 0
                    })
                    .ToListAsync();

                return earnings;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching total earnings by shippers.", ex);
            }
        }

        public async Task<List<ShipperTotalShipmentDto>> GetTotalShipment()
        {

            var shipments = await _context.Orders
                .GroupBy(o => o.ShipVia)
                .Select(g => new
                {
                    ShipperId = g.Key,
                    TotalShipments = g.Count()
                })
                .Join(_context.Shippers,
                      orderGroup => orderGroup.ShipperId,
                      shipper => shipper.ShipperId,
                      (orderGroup, shipper) => new ShipperTotalShipmentDto
                      {
                          CompanyName = shipper.CompanyName,
                          TotalShipment = orderGroup.TotalShipments,
                      })
                .ToListAsync();

            return shipments;
        }

        public async Task<IEnumerable<ResponseShipperDto>> GetAllShippersByCompanyNameAsync(string companyName)
        {
            var shippers = await _context.Shippers
                .Where(s => EF.Functions.Like(s.CompanyName.ToLower(), $"%{companyName.ToLower()}%"))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseShipperDto>>(shippers);
        }
    }
}