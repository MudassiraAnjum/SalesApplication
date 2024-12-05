using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;


namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryController : ControllerBase
    {
        private readonly ITerritoryService _service;
        public TerritoryController(ITerritoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTerritory([FromBody] CreateTerritoryDto territoryDto)
        {
            try
            {
                var responseTerritoryDto = await _service.CreateTerritory(territoryDto);
                return Ok(responseTerritoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}