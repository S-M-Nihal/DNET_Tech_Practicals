using System.Diagnostics;

namespace StudentManagement.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("--> Incoming Request: [{Method}] {Path}",
                context.Request.Method,
                context.Request.Path);

            // Pass execution to the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation("<-- Completed [{Method}] {Path} with Status {StatusCode} in {Elapsed}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}