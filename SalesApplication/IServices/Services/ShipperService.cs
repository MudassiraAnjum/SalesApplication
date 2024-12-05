using AutoMapper;
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
        public async Task<ResponseShipperDto> AddShipperAsync(ShipperDto shipperDto)
        {
            
                var shipper = _mapper.Map<Shipper>(shipperDto);
                await _context.Shippers.AddAsync(shipper);
                await _context.SaveChangesAsync();
                return _mapper.Map<ResponseShipperDto>(shipper);
            
            
        }
        public async Task<ResponseShipperDto> UpdateShipperFullAsync(int shipperId, ShipperDto shipperDto)
        {
            
                var shipper = await _context.Shippers.FindAsync(shipperId);
                if (shipper == null) return null;

                _mapper.Map(shipperDto, shipper);
                await _context.SaveChangesAsync();
                return _mapper.Map<ResponseShipperDto>(shipper);
            
        }
    }
}
