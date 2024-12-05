using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dtos;
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
    }
}
