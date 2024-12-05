using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Data;
using SalesApplication.Dtos;

namespace SalesApplication.IServices.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseOrderDto>> GetOrdersByEmpFNameAsync(string firstName)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.FirstName.ToLower() == firstName.ToLower());

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with first name '{firstName}' not found.");
            }

            var orders = await _dbContext.Orders
                .Where(o => o.EmployeeId == employee.EmployeeId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseOrderDto>>(orders);
        }
    } 
}
