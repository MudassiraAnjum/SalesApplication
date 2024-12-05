using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // Get all orders
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<ActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        // Get all ship details
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("allShipDetails")]
        public async Task<ActionResult> GetAllShipDetailsAsync()
        {
            var shipDetails = await _orderService.GetAllShipDetailsAsync();
            return Ok(shipDetails);
        }
    }
}
