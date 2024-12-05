using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<ResponseOrderDetailDto>> GetAllOrderDetails();

    }
}
