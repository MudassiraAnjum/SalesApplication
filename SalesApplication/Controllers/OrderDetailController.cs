using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;


namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("bill/{employeeId}")]
        public async Task<ActionResult<List<ResponseOrderDetailsDto>>> GetBillAmountByEmployeeId(int employeeId)
        {
            var bills = await _orderDetailService.GetOrderDetailsByEmployeeId(employeeId);
            return Ok(bills);
        }
    }
}
