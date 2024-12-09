using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;
using SalesApplication.Models;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<ResponseShipperDto> CreateShipper(ShipperDto shipperDto);
        Task<List<ResponseShipperDto>> GetAllShipper();
        Task<List<ShipperEarningsDto>> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date);
        Task UpdateShipperAsync(int shipperId, JsonPatchDocument<ShipperUpdateDto> patchDoc);
        Task<List<ShipperEarningsDto>> GetEarningsByShipperAndDateAsync(string companyName, DateTime date);
    }
}
