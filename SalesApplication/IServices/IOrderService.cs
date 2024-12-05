using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        public Task<IEnumerable<ResponseOrderDto>> GetOrder();
    }
}
