using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<ResponseOrderDetailsDto>> GetAllOrderDetails();
        Task<List<ResponseOrderDetailsDto>> GetOrderDetailsByEmployeeId(int employeeId);
    }
}
