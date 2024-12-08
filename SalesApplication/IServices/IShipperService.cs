using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IShipperService
    {
        Task<List<ShipperTotalShipmentDto>> GetTotalShipment();
        Task<List<ShipperTotalAmountDto>> GetTotalAmount();
        Task<IEnumerable<ResponseShipperDto>> GetAllShippersByCompanyNameAsync(string companyName);

    }
}
