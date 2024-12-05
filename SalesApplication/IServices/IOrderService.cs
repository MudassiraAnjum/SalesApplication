using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<ResponseOrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<ShipDetailsDto>> GetAllShipDetailsAsync();

    }
}
