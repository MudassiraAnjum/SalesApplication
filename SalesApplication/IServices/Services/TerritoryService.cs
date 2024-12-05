using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.IServices.Services
{
    public class TerritoryService : ITerritoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TerritoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ResponseTerritoryDto> UpdateTerritoryAsync(string territoryId, TerritoryDto territoryDto)
        {
            
                // Use FirstOrDefaultAsync to search by TerritoryId (string) instead of FindAsync
                var territory = await _context.Territories
                    .FirstOrDefaultAsync(t => t.TerritoryId == territoryId.ToString()); // Ensuring the ID is converted to string if it's an int

                if (territory == null)
                {
                    throw new Exception($"Territory with ID {territoryId} not found.");
                }

                _mapper.Map(territoryDto, territory); // Mapping updated data to the found entity
                _context.Territories.Update(territory);
                await _context.SaveChangesAsync(); // Saving changes

                return _mapper.Map<ResponseTerritoryDto>(territory); // Returning updated territory
            
        }

    }




}
