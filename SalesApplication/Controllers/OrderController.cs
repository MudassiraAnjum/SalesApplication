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

        [Authorize(Roles = "Admin,Employee")]

        [HttpGet("orderbyemployee/{firstName}")]
        public async Task<IActionResult> GetOrdersByEmployeeAsync(string firstName)
        {
                var orders = await _orderService.GetOrdersByEmployeeAsync(firstName);
                return Ok(orders); // Return the mapped order DTOs
        }
    }
}
