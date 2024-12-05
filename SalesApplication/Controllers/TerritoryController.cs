﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IService;

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

        // Create Territory
        [HttpPost]
        public async Task<ActionResult> CreateTerritory([FromBody] TerritoryCreateDto territoryCreateDto)
        {
            try
            {
                var result = await _territoryService.CreateTerritoryAsync(territoryCreateDto);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while creating the territory.");
            }
        }

        [Authorize(Roles = "Admin")]

        // Get All Territories
        [HttpGet]
        public async Task<ActionResult<List<TerritoryResponseDto>>> GetAllTerritories()
        {
            try
            {
                var territories = await _territoryService.GetAllTerritoriesAsync();

                // Trim any leading or trailing spaces from territoryDescription and regionName
                foreach (var territory in territories)
                { 
                    territory.TerritoryDescription = territory.TerritoryDescription?.Trim();
                    territory.RegionName = territory.RegionName?.Trim();  // Trim regionName as well
                }

                return Ok(territories);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while fetching the territories.");
            }
        }

    }
}
