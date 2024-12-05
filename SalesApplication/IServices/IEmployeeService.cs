using SalesApplication.Dtos;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByRegionDescAsync(string regionDescription);

        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByHireDateAsync(DateTime hireDate);

        Task<ResponseEmployeeDto> GetLowestSaleByEmpOnDateAsync(DateTime date);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByDateAsync(DateTime date);
        Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync();
    }
}
