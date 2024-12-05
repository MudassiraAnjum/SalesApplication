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
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;


        public AuthController(AuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> AddShipper([FromBody] RegShipperDto shipperDto)
        {
            var createdShipper = await _authService.RegisterShipperAsync(shipperDto);
            return Ok(createdShipper);

        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var tokenModel = _authService.Authenticate(username, password);
                return Ok(new { token = tokenModel.Token, role = tokenModel.Role });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid username or password");
            }
        }

    }
}