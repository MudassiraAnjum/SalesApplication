﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<string> RegisterShipperAsync(ShipperRegistrationDto registrationDto)
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

        public (string Token, string Role) Authenticate(LoginDto loginDto)
        {
            string role = null;
            int? employeeId = null;
            int? shipperId = null;


            // Check in Admins
            var admins = _configuration.GetSection("Admins").Get<List<Dictionary<string, string>>>();
            var admin = admins.FirstOrDefault(a => a["AdminName"] == loginDto.Username && a["Password"] == loginDto.Password);
            if (admin != null)
            {
                role = "Admin";
            }

            // Check in Employees
            if (role == null)
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.FirstName == loginDto.Username && e.Password == loginDto.Password);
                if (employee != null)
                {
                    role = "Employee";
                    employeeId = employee.EmployeeId; // Get EmployeeId
                }
            }

            // Check in Shippers
            if (role == null)
            {
                var shipper = _dbContext.Shippers.FirstOrDefault(s => s.CompanyName == loginDto.Username && s.Password == loginDto.Password);
                if (shipper != null)
                {
                    role = "Shipper";
                    shipperId=shipper.ShipperId;
                }
            }

            if (role == null)
                return (null, null); // Invalid credentials

            var token = GenerateJwtToken(loginDto.Username, role, employeeId,shipperId);
            return (token, role);
        }

        private string GenerateJwtToken(string username, string role, int? employeeId = null, int? shipperId = null,string firstName = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            // Add EmployeeId claim if the role is Employee
            if (role == "Employee" && employeeId.HasValue)
            {
                claims.Add(new Claim("EmployeeId", employeeId.Value.ToString()));
            }

            // Add firstname claim if provided
            if (!string.IsNullOrEmpty(firstName))
            {
                claims.Add(new Claim("firstname", firstName));
            }

            // Add ShipperId claim if the role is Shipper
            if (role == "Shipper" && shipperId.HasValue)
            {
                claims.Add(new Claim("ShipperId", shipperId.Value.ToString()));
            }


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
