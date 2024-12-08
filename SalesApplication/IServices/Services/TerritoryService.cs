using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using SalesApplication.IServices;


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

        public async Task<ResponseTerritoryDto> AddTerritoryAsync(CreateTerritoryDto territoryDto)
        {
            var territory = _mapper.Map<Territory>(territoryDto);
            _context.Territories.Add(territory);
            await _context.SaveChangesAsync();
            return _mapper.Map<ResponseTerritoryDto>(territory);
        }
    }
}