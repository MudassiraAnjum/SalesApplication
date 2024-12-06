using Microsoft.AspNetCore.Mvc;
using SalesApplication.IServices.Services;
using SalesApplication.Dtos;


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
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var tokenModel = _authService.Authenticate(loginDto);
            if (tokenModel.Token == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(new { token = tokenModel.Token, role = tokenModel.Role });
        }
    }
}
