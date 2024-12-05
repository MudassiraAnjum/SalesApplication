using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.Data;
using SalesApplication.Models;

namespace SalesApplication.IService.Service
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
        public async Task<string> CreateTerritoryAsync(TerritoryCreateDto territoryCreateDto)
        {
            // Check if the TerritoryId already exists
            var existingTerritory = await _context.Territories
                .FirstOrDefaultAsync(t => t.TerritoryId == territoryCreateDto.TerritoryId);

            if (existingTerritory != null)
            {
                return "Territory already exists with the given ID.";
            }

            // If it doesn't exist, create a new territory
            var newTerritory = new Territory
            {
                TerritoryId = territoryCreateDto.TerritoryId,
                TerritoryDescription = territoryCreateDto.TerritoryDescription,
                RegionId = territoryCreateDto.RegionId
            };

            _context.Territories.Add(newTerritory);
            await _context.SaveChangesAsync();

            return "Record Created Successfully";
        }

        public async Task<List<TerritoryResponseDto>> GetAllTerritoriesAsync()
        {
            var territories = await _context.Territories
                .Include(t => t.Region)
                .ToListAsync();

            return _mapper.Map<List<TerritoryResponseDto>>(territories);
        }   
    }
}