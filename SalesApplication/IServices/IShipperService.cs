using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<ResponseShipperDto> CreateShipper(ShipperDto shipperDto);
        Task<List<ResponseShipperDto>> GetAllShipper();
        Task<List<ShipperEarningsDto>> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date);
        Task UpdateShipperAsync(int shipperId, JsonPatchDocument<ShipperUpdateDto> patchDoc);
    }
}
