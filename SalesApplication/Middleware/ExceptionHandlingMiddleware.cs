using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

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
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An exception occurred.");
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred. Please try again later.";

            switch (ex)
            {
                case KeyNotFoundException:
                    // 404 Not Found: Resource does not exist.
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "The specified resource was not found.";
                    break;

                case UnauthorizedAccessException:
                    // 401 Unauthorized: User lacks valid credentials.
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;

                case JsonException:
                    // 400 Bad Request: Malformed JSON in the request body.
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "There was an error processing your request.Data is not correctly formatted as JSON";
                    break;

                case ArgumentNullException argNullEx:
                    // 400 Bad Request: Required argument is null.
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = $"A required parameter was missing: {argNullEx.Message}.";
                    break;

                case ArgumentException argEx:
                    // 400 Bad Request: Invalid argument provided.
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = $"Invalid argument: {argEx.Message}.";
                    break;

                case DbUpdateConcurrencyException:
                    // 409 Conflict: Concurrent database update conflict.
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = "The record you attempted to edit was modified by another user after you retrieved it.";
                    break;

                case DbUpdateException dbUpdateEx:
                    // 400 Bad Request: Error while updating the database.
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "There was an error updating the database.";
                    _logger.LogError(dbUpdateEx, "Database update error.");
                    break;


                case NullReferenceException:
                    // 500 Internal Server Error: Null reference caused a crash.
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "A server error occurred due to a null reference.";
                    break;

                case Exception generalEx:
                    // 500 Internal Server Error: Fallback for unexpected exceptions.
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    _logger.LogError(generalEx, "General exception occurred.");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                Detailed = context.RequestServices.GetService<IHostEnvironment>().IsDevelopment() ? ex.ToString() : null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
