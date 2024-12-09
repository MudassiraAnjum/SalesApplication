using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> DeleteEmployee(int id);
        Task<ResponseEmployeeDto> GetLowestSaleByEmployeeAsync(int year);
        Task<List<EmployeeSalesDto>> GetSalesMadeByEmployeeBetweenDatesAsync(int employeeId, DateTime fromDate, DateTime toDate);
    }
}
