using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dtos;
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
        [HttpGet("search/{companyName}")]
        public async Task<ActionResult<IEnumerable<ResponseShipperDto>>> SearchShipperByCompanyName(string companyName)
        {
            var shippers = await _shipperService.GetAllShippersByCompanyNameAsync(companyName);
            return Ok(shippers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateShipper(int id, [FromBody] JsonPatchDocument<ResponseShipperDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch document.");

            await _shipperService.UpdateShipperAsync(id, patchDoc);
            return NoContent(); // Return 204 No Content if successful
        }
    }
}
