using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.Models;
using SalesApplication.Data;
using SalesApplication.IServices;

namespace Sales_Application.IServices.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ShipperService(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseShipperDto> GetShipperByCompanyNameAsync(string companyName)
        {
            var shipper = await _context.Shippers
                .FirstOrDefaultAsync(s => s.CompanyName.Contains(companyName));

            if (shipper == null) return null;

            return _mapper.Map<ResponseShipperDto>(shipper);
        }

        public async Task<List<ResponseShipperDto>> GetTotalAmountEarnedByShipperAsync(int year)
        {
            return await _context.Orders
                .Where(o => o.ShippedDate.HasValue && o.ShippedDate.Value.Year == year) // Check if the order was shipped in the given year
                .GroupBy(o => o.ShipViaNavigation.CompanyName) // Group by the shipper's company name
                .Select(g => new ResponseShipperDto
                {
                    CompanyName = g.Key,
                    TotalAmount = g.Sum(o => o.Freight ?? 0) // Sum the freight (earnings) of all orders
                })
                .ToListAsync();
        }

    }
}






    