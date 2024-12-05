using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlClient; // For SQL exceptions
using Microsoft.EntityFrameworkCore; // For EF Core exceptions

namespace SalesApplication.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = "Internal Server Error";

            // Handle specific exceptions
            switch (ex)
            {
                case KeyNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = "Resource not found.";
                    break;
                case ArgumentNullException _:
                    code = HttpStatusCode.BadRequest;
                    result = "A required argument was null.";
                    break;
                case JsonException _:
                    code = HttpStatusCode.BadRequest;
                    result = "Invalid JSON format.";
                    break;
                case DbUpdateConcurrencyException _:
                    code = HttpStatusCode.Conflict;
                    result = "Concurrency conflict occurred. Please try again.";
                    break;
                case DbUpdateException dbUpdateEx:
                    code = HttpStatusCode.BadRequest;
                    result = "Database update error. Please check your data.";
                    break;
                case InvalidOperationException _:
                    code = HttpStatusCode.BadRequest;
                    result = "Invalid operation. Please check your request.";
                    break;
                case ArgumentException _:
                    code = HttpStatusCode.BadRequest;
                    result = "Invalid argument provided.";
                    break;
                case Microsoft.Data.SqlClient.SqlException sqlEx: // Ensure this is the correct type
                    code = HttpStatusCode.InternalServerError;
                    result = "Database error occurred. Please contact support.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = result
            }));
        }
    }
}