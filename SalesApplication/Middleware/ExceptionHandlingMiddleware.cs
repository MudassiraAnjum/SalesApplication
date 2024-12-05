using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
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
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Handle the exception
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An exception occurred.");

            // Prepare a default response
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred. Please try again later.";

            // Categorize the exception
            switch (ex)
            {
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "The specified resource was not found.";
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;

                case JsonException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "There was an error processing your request. Please ensure the input is in the correct JSON format.";
                    break;

                case ArgumentNullException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                case DbUpdateConcurrencyException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled.";
                    break;

                case DbUpdateException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "There was an error updating the database. Please check your input and try again.";
                    _logger.LogError(ex, "Database update error.");
                    break;

                default:
                    // For other exceptions, use the generic internal server error
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Set the response status code and content type
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Create the response object
            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                // Detailed error message can be included conditionally based on environment
                Detailed = context.RequestServices.GetService<IHostEnvironment>().IsDevelopment() ? ex.Message : null
            };

            // Write the response
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}










//public class ExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;

//    public ExceptionHandlingMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context); // Continue processing the request
//        }
//        catch (Exception)
//        {
//            // Catch all exceptions and return a consistent error response
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = StatusCodes.Status400BadRequest; // 400 for Validation failure

//            var errorResponse = new
//            {
//                timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd"), // Timestamp
//                message = "Validation failed...." // Generic error message
//            };

//            // Write the response as JSON
//            await context.Response.WriteAsJsonAsync(errorResponse);
//        }
//    }
//}