using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApplication.IServices.Services;
using SalesApplication.Dto;


namespace Sales_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private object _context;

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
   