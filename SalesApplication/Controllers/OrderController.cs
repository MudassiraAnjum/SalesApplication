using Microsoft.AspNetCore.Mvc;
using Sales_Application.IServices;
using AutoMapper;
using SalesApplication.Dto;
using SalesApplication.IServices;
using Microsoft.AspNetCore.Authorization;

public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Endpoint: /api/orders/BetweenDate/{FromDate}/{ToDate}
    [Authorize(Roles = "Admin,Employee")]
    [HttpGet("BetweenDate/{fromDate}/{toDate}")]
    public async Task<IActionResult> GetOrderShipperDetailsBetweenDates(DateTime fromDate, DateTime toDate)
    {
        
            // Call service and return the mapped result
            var result = await _orderService.GetOrderShipperDetailsBetweenDates(fromDate, toDate);
            return Ok(result); // AutoMapper is used inside the service to map data
        
    }

    // Endpoint: /api/orders/shipdetails/{OrderId}
    [Authorize(Roles = "Admin,Employee")]
    [HttpGet("shipdetails/{orderId}")]
    public async Task<IActionResult> GetOrderShipperDetailsByOrderId(int orderId)
    {
        
            var result = await _orderService.GetOrderShipperDetailsByOrderId(orderId);
            return Ok(result);
        
    }
}


