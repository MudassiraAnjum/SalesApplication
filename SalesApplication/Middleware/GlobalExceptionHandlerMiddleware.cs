using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace SalesApplication.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Continue processing the request

                // Handle 403 Forbidden for unauthorized access
                if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden && !httpContext.Response.HasStarted)
                {
                    await HandleForbiddenAsync(httpContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles exceptions and returns a consistent JSON error response.
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set status code using switch expression
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException or ArgumentNullException => StatusCodes.Status400BadRequest,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError // Generic server error
            };

            context.Response.ContentType = "application/json";

            // Construct a simplified error response with just the message
            var response = new
            {
                status = context.Response.StatusCode,
                message = exception.Message
            };

            // Serialize and write the response as JSON
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Handles unauthorized access (403 Forbidden) and returns a JSON response.
        /// </summary>
        private Task HandleForbiddenAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                status = StatusCodes.Status403Forbidden,
                message = "Unauthorized access"
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
