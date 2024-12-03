using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("totalamountearnedbyshipper/{date}")]
        public async Task<IActionResult> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date)
        {
            try
            {
                var result = await _shipperService.GetTotalAmountEarnedByShipperOnDateAsync(date);

                if (result == null || !result.Any())
                    return NotFound("No shipper earnings found for the specified date.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles="Admin,Shipper")]
        [HttpPatch("edit/{shipperId}")]
        public async Task<IActionResult> UpdateShipper(int shipperId, [FromBody] JsonPatchDocument<ShipperUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid input data.");
            }           
            // Call the service to update the shipper with the patch document
            await _shipperService.UpdateShipperAsync(shipperId, patchDoc);
            return NoContent(); // Success, no content to return
        }

    }
}
