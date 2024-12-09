using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        public Task<IEnumerable<ResponseOrderDto>> GetOrder();
        Task<IEnumerable<ResponseOrderDto>> GetOrdersByEmpFNameAsync(string firstName);
        Task<ResponseOrderDto> GetOrderShipperDetailsByOrderId(int orderId);
        Task<IEnumerable<ShipperDetailsDto>> GetAllShipDetailsAsync();
        IEnumerable<OrdersShipDetailsDto> GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate);
        IEnumerable<OrdersByEmployeeDto> GetOrdersCountByEmployee();
    }
}
