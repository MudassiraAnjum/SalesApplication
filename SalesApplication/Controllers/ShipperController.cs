using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.IServices;

namespace SalesApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        [Authorize(Roles = "Admin,Shipper")]
        // API endpoint to get total earnings by shipper on a specific date
        [HttpGet("totalamountearnedbyshipper/{date:datetime}")]
        public async Task<IActionResult> GetTotalAmountEarnedByDate(DateTime date)
        {
            var results = await _shipperService.GetTotalAmountEarnedByShipperAsync(date);
            return Ok(results);
        }

        [Authorize(Roles ="Admin,Shipper")]
        // API endpoint to get total earnings by shipper in a specific year
        [HttpGet("totalamountearnedbyshipper/{year:int}")]
        public async Task<IActionResult> GetTotalAmountEarnedByYear(int year)
        {
            var results = await _shipperService.GetTotalAmountEarnedByShipperAsync(year);
            return Ok(results);
        }
    }
}
