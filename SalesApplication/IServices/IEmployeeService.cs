using Microsoft.AspNetCore.JsonPatch;
using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<ResponseEmployeeDto> GetHighestSaleByEmployeeInYearAsync(int year);
        Task<IEnumerable<ResponseEmployeeDto>> GetEmployeesByCityAsync(string city);
        Task<ResponseEmployeeDto> PatchEmployeeAsync(int employeeId, JsonPatchDocument<EmployeeDto> patchDoc);

    }
}
