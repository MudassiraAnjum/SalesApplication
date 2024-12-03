﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;

namespace SalesApplication.IServices.Services
{
    public class TerritoryService:ITerritoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TerritoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TerritoryDto>> GetTerritory()
        {
                var getterritory = await _context.Territories.ToListAsync();
                return _mapper.Map<List<TerritoryDto>>(getterritory);
        }
    }
}
