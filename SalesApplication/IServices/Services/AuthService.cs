using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SalesApplication.Data;
using SalesApplication.Dto;
using SalesApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesApplication.IServices.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public AuthService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<string> RegisterShipperAsync(RegShipperDto registrationDto)
        {
            // Check if the shipper already exists
            var existingShipper = await _dbContext.Shippers
                .FirstOrDefaultAsync(s => s.CompanyName == registrationDto.CompanyName);

            if (existingShipper != null)
            {
                throw new InvalidOperationException("A shipper with this company name already exists.");
            }

            // Create new shipper
            var newShipper = new Shipper
            {
                CompanyName = registrationDto.CompanyName,
                Email = registrationDto.Email,
                Password = registrationDto.Password // Ensure to hash this password in a real application
            };

            _dbContext.Shippers.Add(newShipper);
            await _dbContext.SaveChangesAsync();

            return "Registered successfully.";
        }

        public (string Token, string Role) Authenticate(string username, string password)
        {
            string role = null;

            // Check in Admins
            var admins = _configuration.GetSection("Admins").Get<List<Dictionary<string, string>>>();
            var admin = admins.FirstOrDefault(a => a["AdminName"] == username && a["Password"] == password);
            if (admin != null)
            {
                role = "Admin";
            }

            // Check in Employees
            if (role == null)
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.FirstName == username && e.Password == password);
                if (employee != null) role = "Employee";
            }

            // Check in Shippers
            if (role == null)
            {
                var shipper = _dbContext.Shippers.FirstOrDefault(s => s.CompanyName == username && s.Password == password);
                if (shipper != null) role = "Shipper";
            }

            if (role == null)
                return (null, null); // Invalid credentials

            var token = GenerateJwtToken(username, role);
            return (token, role);
        }

        private string GenerateJwtToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:TokenLifetimeMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
   
