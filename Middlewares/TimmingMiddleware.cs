using System.Diagnostics;

namespace Api.Middlewares
{
    public class TimmingMiddleware
    {
        public RequestDelegate _next { get; }
        public ILogger<TimmingMiddleware> _logger { get; }

        public TimmingMiddleware(RequestDelegate next, ILogger<TimmingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation($"Request: {context.Request.Path} took '{stopwatch.ElapsedMilliseconds}ms'");


        }

    }
}
