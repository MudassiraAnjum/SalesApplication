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
        [HttpPost]
        public async Task<IActionResult> AddShipper([FromBody] ShipperDto shipperDto)
        {
            var createdShipper = await _shipperService.AddShipperAsync(shipperDto);
            return Ok(createdShipper);
            
        }
        [Authorize(Roles = "Admin,Shipper")]
        [HttpPut("{shipperId}")]
        public async Task<IActionResult> UpdateShipper(int shipperId, [FromBody] ShipperDto shipperDto)
        {
            var updatedShipper = await _shipperService.UpdateShipperFullAsync(shipperId, shipperDto);
            if (updatedShipper == null) return NotFound();
            return Ok(updatedShipper);
        }
    }
}
