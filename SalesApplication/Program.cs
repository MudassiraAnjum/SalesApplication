using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesApplication.Data;
using SalesApplication.IService.Service;
using SalesApplication.IService;
using SalesApplication.IServices;
using SalesApplication.IServices.Services;
//using SalesApplication.Mapping;
using System.Text;
using SalesApplication.Mapper;
using SalesApplication.Middleware;

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
            app.UseMiddleware<ExceptionHandlingMiddleware>();

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


//using SalesApplication.IServices.Services;
//using SalesApplication.IServices;
//using Microsoft.EntityFrameworkCore;
//using SalesApplication.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Builder;
//using SalesApplication.Middleware;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.OpenApi.Models;
//using SalesApplication.Mapper;
//using SalesApplication.Data;
//using SalesApplication.IService.Service;
//using SalesApplication.IService;

//namespace SalesApplication
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddAutoMapper(typeof(SalesMapper));
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.AddScoped<IOrderService, OrderService>();
//            builder.Services.AddScoped<IEmployeeService, EmployeeService>();  // Register the service
//            builder.Services.AddScoped<IShipperService, ShipperService>();
//            builder.Services.AddScoped<ITerritoryService, TerritoryService>();
//            builder.Services.AddControllers();


//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAllOrigins", policy =>
//                {
//                    policy.WithOrigins("https://localhost:7038")
//                          .AllowAnyMethod()
//                          .AllowAnyHeader();
//                });
//            });
//            builder.Services.AddScoped<AuthService>();


//            // Add services to the container
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
//                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                    ValidAudience = builder.Configuration["Jwt:Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(
//                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//                };
//            });
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Salesapplication API", Version = "v1" });

//                // Add JWT Authentication scheme
//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    Type = SecuritySchemeType.ApiKey,
//                    Scheme = "Bearer",
//                    BearerFormat = "JWT",
//                    In = ParameterLocation.Header,
//                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: Bearer eyJhbGciOi...",
//                });

//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    }
//                },
//                new string[] {}
//            }
//        });
//            });




//            builder.Services.AddAuthorization();

//            var app = builder.Build();


//            //Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseCors("AllowFrontend");


//            app.UseStaticFiles(); // Serve static files from wwwroot
//            app.UseMiddleware<ExceptionHandlingMiddleware>();


//            //app.UseHttpsRedirection();
//            app.UseAuthentication();
//            app.UseAuthorization();
//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
