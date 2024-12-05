
using SalesApplication.Dto;
namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        IEnumerable<OrdersShipDetailsDto> GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate);
        IEnumerable<OrdersShipDetailsDto> GetAllShipDetails();
        IEnumerable<OrdersByEmployeeDto> GetOrdersCountByEmployee();
    }
}


