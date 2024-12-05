using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.IServices;

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
            var getterritory=await territoryService.GetTerritory();
            return Ok(getterritory);
        }
    }
}
