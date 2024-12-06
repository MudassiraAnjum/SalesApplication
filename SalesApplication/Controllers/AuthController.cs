using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Dto;
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
        public async Task<IActionResult> Register([FromBody] ShipperRegistrationDto registrationDto)
        {
                var result = await _authService.RegisterShipperAsync(registrationDto);
                return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
                var tokenModel = _authService.Authenticate(username, password);
            return Ok(new { token = tokenModel.Token, role = tokenModel.Role });
        }

    }
}