using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{getid}")]
        public async Task<IActionResult> GetOrderDetailsByEmployeeId(int getid)
        {
            var result = await _orderDetailService.GetOrderDetailById(getid);
            return Ok(result);
        }
    }
}
