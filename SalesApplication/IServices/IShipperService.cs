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
        Task<ResponseShipperDto> UpdateShipperFullAsync(int shipperId, ShipperDto shipperDto);
        Task<IEnumerable<ResponseShipperDto>> GetAllShippersByCompanyNameAsync(string companyName);
        Task<List<ShipperTotalAmountDto>> GetTotalAmount();
        Task<List<ShipperTotalShipmentDto>> GetTotalShipment();
        Task<List<ShipperEarningsResponseDto>> GetTotalAmountEarnedByShipperAsync(int year);
        //Task<Shipper?> GetShipperById(int shipperId);
        //Task<ResponseShipperDto?> GetShipperByCompanyName(string companyName);
        //Task<List<ShipperEarningsDto>> GetEarningsByShipperAndDateAsync(string companyName, DateTime date);
        //Task<Shipper> GetShipperById(int shipperId);
    }
}
