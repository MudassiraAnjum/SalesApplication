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
            try
            {
                var result = await _authService.RegisterShipperAsync(registrationDto);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Register only if you are a new Shipper.If not Login.");
            }
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