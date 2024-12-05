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

        

        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("total-shipments")]
        public async Task<IActionResult> GetTotalShipmentsByShipper()
        {
            var totalShipments = await _shipperService.GetTotalShipment();
            return Ok(totalShipments);
        }

        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("total-earnings")]
        public async Task<IActionResult> GetTotalAmountEarnedByShippers()
        {
            var totalEarnings = await _shipperService.GetTotalAmount();
            return Ok(totalEarnings);
        }

        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("companyname-shippers")]
        public async Task<IActionResult> GetShippersByComapanyName(string company)
        {
            var companyship = await _shipperService.GetByCompanyName(company);
            return Ok(companyship);
        }
    }
}
