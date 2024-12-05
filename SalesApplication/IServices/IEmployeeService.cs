using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<ResponseEmployeeDto>> GetHighestSaleByEmployeeAsync(DateTime date);

        Task<IEnumerable<ResponseEmployeeDto>> GetHighestSaleByEmployeeAsync(int year);

        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByTitleAsync(string title);
        Task<List<OrderInfoDto>> GetSalesByEmployeeOnDateAsync(int employeeId, DateTime date);
    }
}
  
