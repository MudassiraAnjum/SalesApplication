namespace SalesApplication.MiddleWare
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;

        public ExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue processing the request
            }
            catch (Exception)
            {
                // Catch all exceptions and return a consistent error response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest; // 400 for Validation failure

                var errorResponse = new
                {
                    timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd"), // Timestamp
                    message = "Validation failed...." // Generic error message
                };

                // Write the response as JSON
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

}

    