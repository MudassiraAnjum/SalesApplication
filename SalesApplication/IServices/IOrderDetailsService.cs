using SalesApplication.Dtos;

namespace SalesApplication.IServices
{
    public interface IOrderDetailsService
    {
        Task<List<ResponseOrderDetailsDto>> GetOrderDetailsByEmployeeId(int employeeId);
    }
}
