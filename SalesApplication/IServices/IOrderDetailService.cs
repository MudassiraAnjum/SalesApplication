using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderDetailService
    {
        Task<List<ResponseOrderDetailsDto>> GetOrderDetailsByEmployeeId(int employeeId);
    }
}
