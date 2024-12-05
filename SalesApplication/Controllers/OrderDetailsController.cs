using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.IServices;
using SalesApplication.Dtos;

namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailsService;

        public OrderDetailsController(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        [HttpGet("bill/{employeeId}")]
        public async Task<ActionResult<List<ResponseOrderDetailsDto>>> GetBillAmountByEmployeeId(int employeeId)
        {
            var bills = await _orderDetailsService.GetOrderDetailsByEmployeeId(employeeId);
            return Ok(bills);
        }
    }
}
