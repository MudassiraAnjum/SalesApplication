using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<EmployeeCompanyResponseDto> GetEmployeeCompanySalesAsync(int employeeId);
        
    }

}
