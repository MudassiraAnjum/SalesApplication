using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.IServices.Services
{
    public class ShipperService:IShipperService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ShipperService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //Shipper Post
        public async Task<ResponseShipperDto> CreateShipper(ShipperDto shipperDto)
        {
                var createShipper= _mapper.Map<Shipper>(shipperDto);

                await _context.Shippers.AddAsync(createShipper);

                await _context.SaveChangesAsync();

                return _mapper.Map<ResponseShipperDto>(createShipper);
        }

        //Shipper Get
        public async Task<List<ResponseShipperDto>> GetAllShipper()
        {
                var shippers = await _context.Shippers.ToListAsync();
                return _mapper.Map<List<ResponseShipperDto>>(shippers);
        }

        public async Task<List<ShipperEarningsDto>> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date)
        {
                var shipperEarnings = await _context.OrderDetails
                    .Include(od => od.Order)
                    .ThenInclude(o => o.ShipViaNavigation) // Include Shipper details
                    .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Date == date.Date)
                    .GroupBy(od => od.Order.ShipViaNavigation) // Group by Shipper object
                    .Select(g => new ShipperEarningsDto
                    {
                        CompanyName = g.Key.CompanyName ?? "Unknown Shipper", // Access Shipper's CompanyName // Access Shipper's CompanyName
                        TotalAmount = g.Sum(od =>
                            od.UnitPrice *
                            (decimal)od.Quantity *  // Convert Quantity to decimal for multiplication
                            (1 - (decimal)od.Discount))  // Convert Discount to decimal
                    })
                    .ToListAsync();

                return shipperEarnings;
        }

        public async Task UpdateShipperAsync(int shipperId, JsonPatchDocument<ShipperUpdateDto> patchDoc)
        {
            var shipper = await _context.Shippers
                .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

            if (shipper == null)
                throw new Exception($"Shipper with ID {shipperId} not found.");

            // Map the existing shipper data to a DTO object to apply the patch
            var shipperDto = new ShipperUpdateDto
            {
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone,
                Email = shipper.Email
            };

            // Apply the patch document to the DTO
            patchDoc.ApplyTo(shipperDto);

            // Update the shipper entity with the patched values
            if (!string.IsNullOrEmpty(shipperDto.CompanyName))
            {
                shipper.CompanyName = shipperDto.CompanyName;
            }
            if (!string.IsNullOrEmpty(shipperDto.Phone))
            {
                shipper.Phone = shipperDto.Phone;
            }
            if (!string.IsNullOrEmpty(shipperDto.Email))
            {
                shipper.Email = shipperDto.Email;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
            public async Task<ResponseShipperDto?> GetShipperByCompanyName(string companyName)
            {
                var shipper = await _context.Shippers.FirstOrDefaultAsync(s => s.CompanyName == companyName);

                if (shipper == null) return null;

                return _mapper.Map<ResponseShipperDto>(shipper);
            }
        public async Task<List<ShipperEarningsDto>> GetEarningsByShipperAndDateAsync(string companyName, DateTime date)
        {
            var earnings = await _context.OrderDetails
                .Include(od => od.Order)
                .ThenInclude(o => o.ShipViaNavigation) // Include Shipper details
                .Where(od =>
                    od.Order.OrderDate.HasValue &&
                    od.Order.OrderDate.Value.Date == date.Date &&
                    od.Order.ShipViaNavigation.CompanyName == companyName) // Match the logged-in shipper
                .Select(od => new ShipperEarningsDto
                {
                    CompanyName = companyName,
                    TotalAmount = od.UnitPrice *
                                  (decimal)od.Quantity *
                                  (1 - (decimal)od.Discount)
                })
                .ToListAsync();

            return earnings;
        }

        public async Task<Shipper> GetShipperById(int shipperId)
        {
            // Retrieve the shipper by their ID
            return await _context.Shippers
                                 .Where(s => s.ShipperId == shipperId)
                                 .FirstOrDefaultAsync();
        }
    }
}

