using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;
using System.Security.Claims;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByRegionDescAsync(string regionDescription);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByHireDateAsync(DateTime hireDate);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByDateAsync(DateTime date);
        Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync();
        Task<ResponseEmployeeDto> GetHighestSaleByEmployeeAsync(DateTime date);
        Task<ResponseEmployeeDto> GetHighestSaleByEmployeeAsync(int year);

        Task<ResponseEmployeeDto> GetLowestSaleByEmployeeAsync(DateTime date);
        Task<ResponseEmployeeDto> GetLowestSaleByEmployeeAsync(int year);

        Task<List<EmployeeSalesDto>> GetSalesMadeByEmployeeBetweenDatesAsync(DateTime fromDate, DateTime toDate);
        Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<ResponseEmployeeDto> patch);

        Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<(string CompanyName, decimal TotalSales)> GetEmployeeCompanySalesAsync(int employeeId);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByCityAsync(string city);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByTitleAsync(string title);
        Task<IEnumerable<EmployeeSalesDto>> SalesMadeByEmployeeDate(int empid, DateTime date);
        Task<EmployeeDto> UpdateEmployeeByAsync(int employeeId, EmployeeDto employeeDto, ClaimsPrincipal user);
    }
}
