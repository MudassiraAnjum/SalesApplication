using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using System.Security.Claims;

namespace SalesApplication.IServices.Services
{
    public class ShipperService:IShipperService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShipperService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
            var user = _httpContextAccessor.HttpContext?.User;
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == "Admin")
            {
                // Admin can view all shippers
                var shippers = await _context.Shippers.ToListAsync();
                return _mapper.Map<List<ResponseShipperDto>>(shippers);
            }

            if (role == "Shipper")
            {
                // Shipper can only view their own data
                var shipperId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "ShipperId")?.Value ?? "-1");
                var shipper = await _context.Shippers.Where(s => s.ShipperId == shipperId).ToListAsync();
                return _mapper.Map<List<ResponseShipperDto>>(shipper);
            }

            return null; // Unauthorized
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

        public async Task<ResponseShipperDto> UpdateShipperFullAsync(int shipperId, ShipperDto shipperDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == "Admin" ||
                (role == "Shipper" && int.Parse(user.Claims.FirstOrDefault(c => c.Type == "ShipperId")?.Value ?? "-1") == shipperId))
            {
                var shipper = await _context.Shippers.FindAsync(shipperId);
                if (shipper == null)
                {
                    throw new UnauthorizedAccessException("You cannot edit details of another shipper.");
                }
                _mapper.Map(shipperDto, shipper);
                await _context.SaveChangesAsync();
                return _mapper.Map<ResponseShipperDto>(shipper);
            }

            throw new UnauthorizedAccessException("You are not authorized to update this shipper's details.");
        }

        public async Task UpdateShipperAsync(int shipperId, JsonPatchDocument<ShipperUpdateDto> patchDoc)
        {
            // Get the current user from HttpContext (via _httpContextAccessor)
            var user = _httpContextAccessor.HttpContext?.User;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userShipperIdClaim = user?.FindFirst("ShipperId")?.Value;

            // Restrict access based on role and claims
            if (role == "Shipper" && (!int.TryParse(userShipperIdClaim, out var userShipperId) || userShipperId != shipperId))
            {
                throw new UnauthorizedAccessException("You are not authorized to update this shipper's details.");
            }

            // Retrieve the shipper entity to update
            var shipper = await _context.Shippers.FindAsync(shipperId);

            // If the shipper is not found, throw an exception
            if (shipper == null)
            {
                throw new KeyNotFoundException($"Shipper with ID {shipperId} not found.");
            }

            // Map the existing shipper data to a DTO object for patching
            var shipperDto = _mapper.Map<ShipperUpdateDto>(shipper);

            // Apply the patch document to the DTO
            patchDoc.ApplyTo(shipperDto);

            // Update the shipper entity with the patched values
            _mapper.Map(shipperDto, shipper);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }



        //public async Task<Shipper?> GetShipperById(int shipperId)
        //{
        //    // Fetch the shipper by ID from the database
        //    var shipper = await _context.Shippers
        //        .FirstOrDefaultAsync(s => s.ShipperId == shipperId);

        //    return shipper; // Return the shipper if found, otherwise null
        //}


     
    }
}

