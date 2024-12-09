using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sales_Application.IServices.Services;
using Sales_Application.IServices;
using SalesApplication.Data;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;
using System.Text;
using SalesApplication.MiddleWare;
using SalesApplication.Mapper;

namespace SalesApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext to the container
            builder.Services.AddDbContext<ApplicationDbContext>(db =>
                db.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add JWT Authentication
            var secretKey = builder.Configuration["Jwt:Key"];
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(SalesMapper));

            // Add application services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ITerritoryService, TerritoryService>();
            builder.Services.AddScoped<IShipperService, ShipperService>();
            builder.Services.AddScoped<AuthService>();

            // Add controllers with JSON support
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                });
            });

            // Add Authorization
            builder.Services.AddAuthorization();

            // Configure Swagger with JWT support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer [space] your_token_here'"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use CORS
            app.UseCors("AllowAllOrigins");

            // Register the exception handling middleware
            app.UseMiddleware<ExceptionHandling>();

            // Serve static files
            app.UseStaticFiles();

            // Use Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}

//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using SalesApplication.IServices.Services;
//using SalesApplication.IServices;
//using System.Text;
//using SalesApplication.MiddleWare;
//using SalesApplication.Mapper;
//using SalesApplication.Data;
//using Sales_Application.IServices;
//using Sales_Application.IServices.Services;

//namespace SalesApplication
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add DbContext to the container
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//            // Add JWT Authentication
//            var secretKey = builder.Configuration["Jwt:Key"];
//            var issuer = builder.Configuration["Jwt:Issuer"];
//            var audience = builder.Configuration["Jwt:Audience"];

//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = issuer,
//                    ValidAudience = audience,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
//                };
//            });

//            // Add AutoMapper
//            builder.Services.AddAutoMapper(typeof(SalesMapper));

//            // Add application services
//            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//            builder.Services.AddScoped<IOrderService, OrderService>();
//            builder.Services.AddScoped<ITerritoryService, TerritoryService>();
//            builder.Services.AddScoped<IShipperService, ShipperService>();
//            builder.Services.AddScoped<AuthService>();

//            // Add controllers with JSON support
//            builder.Services.AddControllers().AddNewtonsoftJson();

//            // Configure CORS
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAll", builder =>
//                    builder.AllowAnyOrigin()
//                           .AllowAnyMethod()
//                           .AllowAnyHeader());
//            });

//            // Add Authorization
//            builder.Services.AddAuthorization();

//            // Configure Swagger with JWT support
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    Type = SecuritySchemeType.Http,
//                    Scheme = "Bearer",
//                    BearerFormat = "JWT",
//                    In = ParameterLocation.Header,
//                    Description = "Enter 'Bearer [space] your_token_here'"
//                });

//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                        new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        Array.Empty<string>()
//                    }
//                });
//            });

//            var app = builder.Build();

//            // Configure the HTTP request pipeline
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            // Use CORS
//            app.UseCors("AllowAll");

//            // Register the exception handling middleware
//            app.UseMiddleware<ExceptionHandling>();

//            // Serve static files
//            app.UseStaticFiles();

//            // Use Authentication & Authorization
//            app.UseAuthentication();
//            app.UseAuthorization();

//            // Map controllers
//            app.MapControllers();

//            // Run the application
//            app.Run();
//        }
//    }
//}
