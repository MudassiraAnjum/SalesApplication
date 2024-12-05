using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
using SalesApplication.IServices;

namespace Sales_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        private readonly IMapper _mapper;

        public ShipperController(IShipperService shipperService, IMapper mapper)
        {
            _shipperService = shipperService;
            _mapper = mapper;
        }

        // Get Shipper by CompanyName
        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("{companyName}")]
        public async Task<IActionResult> GetShipperByCompanyName(string companyName)
        {
            var shipper = await _shipperService.GetShipperByCompanyNameAsync(companyName);

            if (shipper == null)
            {
                return NotFound(); // Return 404 if the shipper is not found
            }
            var shipperDto = _mapper.Map<ResponseShipperDto>(shipper);
            return Ok(shipperDto);
        }
        //API endpoint to get total earnings by shipper in a specific year
        [Authorize(Roles = "Admin,Shipper")]
        [HttpGet("totalamountearnedbyshipper/{year:int}")]
        public async Task<IActionResult> GetTotalAmountEarnedByYear(int year)
        {
            var results = await _shipperService.GetTotalAmountEarnedByShipperAsync(year);
            return Ok(results);
        }
    }
}



