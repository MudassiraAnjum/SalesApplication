using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;


namespace Sales_Application.Controllers
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
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{territoryId}")]
        public async Task<IActionResult> UpdateTerritory(string territoryId, [FromBody] TerritoryUpdateDto territoryDto)
        {
                // Call service method to update the territory
                var updatedTerritory = await _territoryService.UpdateTerritoryAsync(territoryId, territoryDto);

                // Return the updated territory with a 200 OK response
                return Ok(updatedTerritory); // 200 OK with updated data
            
        }


    }
}