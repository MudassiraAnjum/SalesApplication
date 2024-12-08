using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using System.Net;
using System.Security.Claims;

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
        [HttpPost]
        public async Task<IActionResult> CreateShipper(ShipperDto shipperDto)
        {
            var create = await _shipperService.CreateShipper(shipperDto);
            return Ok(create);
        }

        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet]
        public async Task<IActionResult> GetAllShipper()
        {
            var getAll = await _shipperService.GetAllShipper();
            return Ok(getAll);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("totalamountearnedbyshipper/{date}")]
        public async Task<IActionResult> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date)
        {
            var result = await _shipperService.GetTotalAmountEarnedByShipperOnDateAsync(date);

            if (result == null || !result.Any())
                return NotFound("No shipper earnings found for the specified date.");

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Shipper")]
        [HttpPut("{shipperId}")]
        public async Task<IActionResult> UpdateShipper(int shipperId, [FromBody] ShipperDto shipperDto)
        {
            var updatedShipper = await _shipperService.UpdateShipperFullAsync(shipperId, shipperDto);
            if (updatedShipper == null) return NotFound();
            return Ok(updatedShipper);
        }


        [Authorize(Roles = "Admin,Shipper")]
        [HttpPatch("edit/{shipperId}")]
        public async Task<IActionResult> UpdateShipper(int shipperId, [FromBody] JsonPatchDocument<ShipperUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid input data.");
            }

            // Call the service to handle the update logic
            await _shipperService.UpdateShipperAsync(shipperId, patchDoc);

            return NoContent(); // Return 204 No Content on successful update
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search/{companyName}")]
        public async Task<ActionResult<IEnumerable<ResponseShipperDto>>> SearchShipperByCompanyName(string companyName)
        {
            var shippers = await _shipperService.GetAllShippersByCompanyNameAsync(companyName);
            return Ok(shippers);
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
        // API endpoint to get total earnings by shipper in a specific year
        [HttpGet("totalamountearnedbyshipper/{year:int}")]
        public async Task<IActionResult> GetTotalAmountEarnedByYear(int year)
        {
            var results = await _shipperService.GetTotalAmountEarnedByShipperAsync(year);
            return Ok(results);
        }
    }
}
