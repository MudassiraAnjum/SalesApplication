using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersByEmployeeAsync(string firstName);
    }
}
