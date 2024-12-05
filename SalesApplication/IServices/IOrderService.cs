using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;

namespace SalesApplication.IServices
{
    public interface IOrderService
    {
        Task<List<ResponseOrderDto>> GetOrderShipperDetailsBetweenDates(DateTime fromDate, DateTime toDate);
        Task<ResponseOrderDto> GetOrderShipperDetailsByOrderId(int orderId);
    }
}