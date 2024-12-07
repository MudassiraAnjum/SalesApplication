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

        public async Task<ResponseTerritoryDto> AddTerritoryAsync(TerritoryDto territoryDto)
        {
            var territory = _mapper.Map<Territory>(territoryDto);
            _context.Territories.Add(territory);
            await _context.SaveChangesAsync();
            return _mapper.Map<ResponseTerritoryDto>(territory);
        }

        public async Task<List<ResponseTerritoryDto>> GetTerritory()
        {
            var getterritory = await _context.Territories.ToListAsync();
            var territoryDtos = _mapper.Map<List<ResponseTerritoryDto>>(getterritory);

            // Trim TerritoryDescription for each DTO
            foreach (var territory in territoryDtos)
            {
                territory.TerritoryDescription = territory.TerritoryDescription.Trim();
            }

            return territoryDtos;
        }

       
    }
}