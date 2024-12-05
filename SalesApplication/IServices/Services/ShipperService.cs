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
            //try
            //{
                var createShipper= _mapper.Map<Shipper>(shipperDto);

                await _context.Shippers.AddAsync(createShipper);

                await _context.SaveChangesAsync();

                return _mapper.Map<ResponseShipperDto>(createShipper);
            //}
            //catch (DbUpdateException dbEx)
            //{
            //    throw new Exception("A database error occurred while saving the shipper. Please ensure all data is valid.", dbEx);
            //}
            //catch (AutoMapperMappingException mapEx)
            //{
            //    throw new Exception("An error occurred during the mapping process. Please check the DTO and entity mapping configuration.", mapEx);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("An unexpected error occurred. Please contact support.", ex);
            //}
        }

        //Shipper Get
        public async Task<List<ResponseShipperDto>> GetAllShipper()
        {
            //try
            //{
                var shippers = await _context.Shippers.ToListAsync();
                return _mapper.Map<List<ResponseShipperDto>>(shippers);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("An error occurred while fetching all shippers.", ex);
            //}
        }

        public async Task<List<ShipperEarningsDto>> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date)
        {
            //try
            //{
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
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("An error occurred while fetching total earnings for shippers on the specified date.", ex);
            //}
        }

        public async Task UpdateShipperAsync(int shipperId, JsonPatchDocument<ShipperUpdateDto> patchDoc)
        {
            //try
            //{
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
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("An error occurred while updating the shipper details.", ex);
            //}
        }
    }
}
