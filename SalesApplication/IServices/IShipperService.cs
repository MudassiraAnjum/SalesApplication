using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<ResponseShipperDto> AddShipperAsync(ShipperDto shipperDto);
        Task<ResponseShipperDto> UpdateShipperFullAsync(int shipperId, ShipperDto shipperDto);
    }
}
