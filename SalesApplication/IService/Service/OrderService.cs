using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.Models;
using SalesApplication.Data;
using System.Collections.Generic;
using System.Linq;
using SalesApplication.Dto;
using SalesApplication.IServices;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public IEnumerable<OrdersShipDetailsDto> GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate)
    {
        var orders = _context.Orders
            .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
            .ToList();

        return _mapper.Map<IEnumerable<OrdersShipDetailsDto>>(orders);
    }

    public IEnumerable<OrdersShipDetailsDto> GetAllShipDetails()
    {
        var orders = _context.Orders.ToList();
        return _mapper.Map<IEnumerable<OrdersShipDetailsDto>>(orders);
    }

    public IEnumerable<OrdersByEmployeeDto> GetOrdersCountByEmployee()
    {
        // Ensure ShipName and ShipAddress are properly handled as strings (e.g., using .ToString() if they are ntext)
        return _context.Orders
     .GroupBy(o => o.EmployeeId)  // Group by EmployeeId (or EmployeeName, depending on your schema)
     .Select(group => new OrdersByEmployeeDto
     {
         // Assuming Employee has FirstName and LastName properties
         EmployeeName = group.FirstOrDefault().Employee.FirstName + " " + group.FirstOrDefault().Employee.LastName,
         TotalOrders = group.Count()
     })
     .ToList();

    }
}
    