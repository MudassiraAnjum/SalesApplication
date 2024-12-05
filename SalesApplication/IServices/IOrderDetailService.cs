using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderDetailService
    {
        public Task<IEnumerable<ResponseOrderDetailsDto>> GetOrderDetailById(int id);
    }
}
