using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;

namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetOrder();
            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("employee/{firstName}")]
        public async Task<ActionResult<IEnumerable<ResponseOrderDto>>> GetOrdersByEmployee(string firstName)
        {
            var orders = await _orderService.GetOrdersByEmpFNameAsync(firstName);
            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No orders found for the specified employee." });
            }
            return Ok(orders);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("shipdetails/{orderId}")]
        //public async Task<IActionResult> GetOrderShipperDetailsByOrderId(int orderId)
        //{

        //    var result = await _orderService.GetOrderShipperDetailsByOrderId(orderId);
        //    return Ok(result);

        //}
        [Authorize(Roles = "Admin")]
        [HttpGet("allShipDetails")]
        public async Task<ActionResult> GetAllShipDetailsAsync()
        {
            var shipDetails = await _orderService.GetAllShipDetailsAsync();
            return Ok(shipDetails);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("BetweenDate/{fromDate}/{toDate}")]
        public IActionResult GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate)
        {

            var results = _orderService.GetShipDetailsBetweenDates(fromDate, toDate);
            return Ok(results);

        }
        [Authorize(Roles = "Admin")]

        [HttpGet("numberoforderbyeachemployee")]
        public IActionResult GetNumberOfOrdersByEachEmployee()
        {
            var results = _orderService.GetOrdersCountByEmployee();
            return Ok(results);
        }
    }
}
