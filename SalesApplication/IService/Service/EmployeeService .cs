using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.Models;
using SalesApplication.Data;
public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Get all employees and map to EmployeeResponseDto using AutoMapper
    public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees.ToListAsync();

        // Use AutoMapper to map list of Employee to EmployeeResponseDto
        var employeeDtos = _mapper.Map<List<EmployeeResponseDto>>(employees);
        return employeeDtos;
    }

    public async Task<EmployeeCompanyResponseDto> GetEmployeeCompanySalesAsync(int employeeId)
    {
        
        var employee = await _context.Employees
            .Include(e => e.Orders)
            .ThenInclude(o => o.OrderDetails)
            .Include(e => e.Orders)
            .ThenInclude(o => o.ShipViaNavigation)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found.");

        var totalSales = employee.Orders?.Sum(o => o.OrderDetails
            .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))) ?? 0;

        var companyName = employee.Orders?.FirstOrDefault()?.ShipViaNavigation?.CompanyName ?? "No company available";

        return new EmployeeCompanyResponseDto
        {
            CompanyName = companyName,
            TotalSales = totalSales
        };
    }
}

        //catch (KeyNotFoundException ex)
        //{
        //    throw new Exception($"Employee with ID {employeeId} not found. {ex.Message}");
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception($"An error occurred while retrieving sales data: {ex.Message}");
 