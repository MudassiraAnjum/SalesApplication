using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<ResponseEmployeeDto> UpdateEmployee(int empid, CreateEmployeeDto createemployeedto);
        Task<ResponseEmployeeDto> PatchUpdateEmployee(int empid, JsonPatchDocument<CreateEmployeeDto> patch);
        Task<IEnumerable<SalesEmployeeDateDto>> SalesMadeByEmployeeDate(int empid, DateTime date);
    }
}
