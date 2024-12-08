using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
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


        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("totalamountearnedbyshipper/{date}")]
        public async Task<IActionResult> GetTotalAmountEarnedByShipperOnDateAsync(DateTime date)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userName = User.Identity?.Name;

            if (userRole == "Shipper")
            {
                var earnings = await _shipperService.GetEarningsByShipperAndDateAsync(userName, date);

                if (earnings == null || !earnings.Any())
                {
                    return NotFound("No sales made by the logged-in shipper on the specified date.");
                }

                return Ok(earnings);
            }

            // If Admin, fetch all earnings
            var allEarnings = await _shipperService.GetTotalAmountEarnedByShipperOnDateAsync(date);
            return Ok(allEarnings);
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

    }
}
