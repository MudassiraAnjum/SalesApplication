using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesApplication.IServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> DeleteEmployee(int id)
        {
            try
            {
                // Find the employee by id
                var deleteEmp = await _context.Employees.FindAsync(id);

                if (deleteEmp == null)
                {
                    throw new KeyNotFoundException("Employee not found. Please check the provided Employee ID.");
                }

                // Remove the employee from the context
                _context.Employees.Remove(deleteEmp);

                // Save changes to the database
                await _context.SaveChangesAsync();
                return _mapper.Map<EmployeeDto>(deleteEmp);
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidOperationException($"Deletion failed: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the database. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<EmployeeDto> GetLowestSaleByEmpInYearAsync(int year)
        {
            try
            {
                var sales = await _context.OrderDetails
                .Include(od => od.Order)
                    .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Year == year)
                    .GroupBy(od => od.Order.EmployeeId)
                    .Select(g => new
                    {
                        EmployeeId = g.Key,
                        TotalSale = g.Sum(od =>
                            od.UnitPrice *
                            (decimal)od.Quantity *  // Convert Quantity to decimal for multiplication
                            (1 - (decimal)od.Discount))  // Convert Discount to decimal
                    })
                    .OrderBy(x => x.TotalSale)
                    .FirstOrDefaultAsync();

                if (sales == null) return null; // No sales found for that date

                var employee = await _context.Employees.FindAsync(sales.EmployeeId);
                return _mapper.Map<EmployeeDto>(employee);
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the employee with the lowest sale in the specified year.", ex);
            }
        }

        public async Task<List<EmployeeSalesDto>> GetSalesMadeByEmployeeBetweenDatesAsync(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var salesData = await _context.Orders
                    .Include(o => o.ShipViaNavigation) // Include Shipper details to get CompanyName
                    .Where(o => o.EmployeeId == employeeId &&
                                o.OrderDate >= fromDate &&
                                o.OrderDate <= toDate)
                    .Select(o => new EmployeeSalesDto
                    {
                        OrderId = o.OrderId,
                        CompanyName = o.ShipViaNavigation.CompanyName ?? "Unknown Shipper"
                    })
                    .ToListAsync();

                return salesData;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales data for the employee.", ex);
            }
        }


    }
}
