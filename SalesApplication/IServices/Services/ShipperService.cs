using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dtos;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IEnumerable<ResponseShipperDto>> GetAllShippersByCompanyNameAsync(string companyName)
        {
            var shippers = await _context.Shippers
                .Where(s => EF.Functions.Like(s.CompanyName.ToLower(), $"%{companyName.ToLower()}%"))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseShipperDto>>(shippers);
        }

        public async Task UpdateShipperAsync(int id, JsonPatchDocument<ResponseShipperDto> patchDoc)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null)
            {
                throw new KeyNotFoundException($"Shipper with ID {id} not found.");
            }

            // Map the entity to a DTO
            var shipperDto = _mapper.Map<ResponseShipperDto>(shipper);

            // Apply the patch
            patchDoc.ApplyTo(shipperDto);

            // Validate the patched DTO
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(shipperDto, null, null);
            if (!Validator.TryValidateObject(shipperDto, validationContext, validationResults, true))
            {
                var errorMessages = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
                throw new InvalidOperationException($"Validation failed: {errorMessages}");
            }

            // Map the patched DTO back to the entity
            _mapper.Map(shipperDto, shipper);

            // Save the changes
            await _context.SaveChangesAsync();
        }
    }
}
