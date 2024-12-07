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
        private readonly ITerritoryService territoryService;
        public TerritoryController(ITerritoryService _territoryService)
        {
            territoryService = _territoryService;
        }

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> getAllTerritory()
        {
            var getterritory = await territoryService.GetTerritory();
            return Ok(getterritory);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTerritory([FromBody] TerritoryDto territoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var responseTerritoryDto = await territoryService.AddTerritoryAsync(territoryDto);
            return Ok(responseTerritoryDto);
        }
    }
}