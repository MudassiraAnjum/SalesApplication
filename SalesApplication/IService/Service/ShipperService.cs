using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.Models;
using SalesApplication.Data;
using SalesApplication.Dto;

public class ShipperService : IShipperService
{
    private readonly ApplicationDbContext _context;

    public ShipperService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get the total amount earned by each shipper on a specific date
    public async Task<List<ShipperEarningsResponseDto>> GetTotalAmountEarnedByShipperAsync(DateTime date)
    {
        return await _context.Orders
            .Where(o => o.ShippedDate.HasValue && o.ShippedDate.Value.Date == date.Date) // Check if the order was shipped on the given date
            .GroupBy(o => o.ShipViaNavigation.CompanyName) // Group by the shipper's company name
            .Select(g => new ShipperEarningsResponseDto
            {
                CompanyName = g.Key,
                TotalAmount = g.Sum(o => o.Freight ?? 0) // Sum the freight (earnings) of all orders
            })
            .ToListAsync();
    }

    // Get the total amount earned by each shipper in a specific year
    public async Task<List<ShipperEarningsResponseDto>> GetTotalAmountEarnedByShipperAsync(int year)
    {
        return await _context.Orders
            .Where(o => o.ShippedDate.HasValue && o.ShippedDate.Value.Year == year) // Check if the order was shipped in the given year
            .GroupBy(o => o.ShipViaNavigation.CompanyName) // Group by the shipper's company name
            .Select(g => new ShipperEarningsResponseDto
            {
                CompanyName = g.Key,
                TotalAmount = g.Sum(o => o.Freight ?? 0) // Sum the freight (earnings) of all orders
            })
            .ToListAsync();
    }
}
