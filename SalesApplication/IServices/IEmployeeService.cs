using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;
using System.Security.Claims;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<ResponseEmployeeDto> UpdateEmployeeByAsync(int employeeId, CreateEmployeeDto employeeDto, ClaimsPrincipal user);
        Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<ResponseEmployeeDto> patch);
        Task<IEnumerable<SalesEmployeeDateDto>> SalesMadeByEmployeeDate(int empid, DateTime date);
    }
}
