using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;

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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetails();
            return Ok(orderDetails);
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
