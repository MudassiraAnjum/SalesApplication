using SalesApplication.Dtos;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<ResponseOrderDto>> GetOrdersByEmpFNameAsync(string firstName);
    }
}
