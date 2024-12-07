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

        //// If Admin, fetch all shippers
        //var allShippers = await _shipperService.GetAllShipper();
        //    return Ok(allShippers);
        //}


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

            // Get the logged-in user's role and username
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userName = User.Identity?.Name;  // Assuming userName is the CompanyName for the Shipper role

            // If the logged-in user is a shipper, check if the user is trying to update their own data
            if (userRole == "Shipper" && userName != null)
            {
                // Fetch the shipper data by shipperId to compare
                var shipper = await _shipperService.GetShipperById(shipperId);

                if (shipper == null)
                {
                    return NotFound("Shipper not found.");
                }

                // Ensure the logged-in shipper can only update their own data
                if (shipper.CompanyName != userName)
                {
                    return Forbid("You cannot update another shipper's data.");
                }
            }

            // Call the service to update the shipper with the patch document
            await _shipperService.UpdateShipperAsync(shipperId, patchDoc);
            return NoContent(); // Success, no content to return

        }

    }
}
