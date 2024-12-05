using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dtos;
using SalesApplication.IServices;

namespace SalesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryController : ControllerBase
    {
        private readonly ITerritoryService _territoryService;

        public TerritoryController(ITerritoryService territoryService)
        {
            _territoryService = territoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTerritory([FromBody] TerritoryDto territoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var responseTerritoryDto = await _territoryService.AddTerritoryAsync(territoryDto);
            return Ok(responseTerritoryDto);
        }
    }
}
