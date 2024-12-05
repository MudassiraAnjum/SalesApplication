using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
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
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{territoryId}")]
        public async Task<IActionResult> UpdateTerritory(string territoryId, [FromBody] TerritoryDto territoryDto)
        {
            try
            {
                // Call service method to update the territory
                var updatedTerritory = await _territoryService.UpdateTerritoryAsync(territoryId, territoryDto);

                // Return the updated territory with a 200 OK response
                return Ok(updatedTerritory); // 200 OK with updated data
            }
            catch (Exception ex)
            {
                // If error occurs, return a 400 BadRequest with the error message
                return BadRequest(ex.Message);
            }
        }


    }
}
