using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;


namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("total-shipments")]
        public async Task<IActionResult> GetTotalShipmentsByShipper()
        {
            var totalShipments = await _shipperService.GetTotalShipment();
            return Ok(totalShipments);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total-earnings")]
        public async Task<IActionResult> GetTotalAmountEarnedByShippers()
        {
            var totalEarnings = await _shipperService.GetTotalAmount();
            return Ok(totalEarnings);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search/{companyName}")]
        public async Task<ActionResult<IEnumerable<ResponseShipperDto>>> SearchShipperByCompanyName(string companyName)
        {
            var shippers = await _shipperService.GetAllShippersByCompanyNameAsync(companyName);
            return Ok(shippers);
        }
    }
}
