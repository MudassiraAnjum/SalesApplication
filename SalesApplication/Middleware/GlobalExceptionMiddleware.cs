using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; // For EF exceptions

namespace SalesApplication.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(context, ex); // Handle exceptions
            }
        }

        private Task HandleErrorAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An exception occurred during the request processing.");

            // Set default response properties
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred while processing your request.";

            // Handle specific exceptions
            switch (ex)
            {
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "The resource you are looking for could not be found.";
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = "You do not have permission to perform this action.";
                    break;

                case JsonException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "There was an error with the JSON data. Please ensure it's correctly formatted.";
                    break;

                case ArgumentNullException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Required input is missing. Please check the provided data.";
                    break;

                case DbUpdateConcurrencyException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = "The record you attempted to edit was modified by another user. The update operation was canceled.";
                    break;

                case DbUpdateException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "A database error occurred. Please check the input data and try again.";
                    _logger.LogError(ex, "Database update error.");
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected server error occurred. Please try again later.";
                    break;
            }

            // Set response content type and status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Prepare the response body
            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                // Include detailed error message if in development environment
                Detailed = context.RequestServices.GetService<IHostEnvironment>().IsDevelopment() ? ex.Message : null
            };

            // Write the response to the client
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
