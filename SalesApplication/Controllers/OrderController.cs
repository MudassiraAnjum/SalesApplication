using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;

namespace SalesApplication.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin,Employee")]

        [HttpGet("BetweenDate/{fromDate}/{toDate}")]
        public IActionResult GetShipDetailsBetweenDates(DateTime fromDate, DateTime toDate)
        {
            var results = _orderService.GetShipDetailsBetweenDates(fromDate, toDate);
            return Ok(results);
        }

        [Authorize(Roles = "Admin,Employee")]

        [HttpGet("allshipdetails")]
        public IActionResult GetAllShipDetails()
        {
            var results = _orderService.GetAllShipDetails();
            return Ok(results);
        }

        [Authorize(Roles = "Admin,Employee")]

        [HttpGet("numberoforderbyeachemployee")]
        public IActionResult GetNumberOfOrdersByEachEmployee()
        {
            var results = _orderService.GetOrdersCountByEmployee();
            return Ok(results);
        }
    }
}

    