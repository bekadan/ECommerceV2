using ExceptionHandling.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace ExceptionHandling.Core.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if incoming request has a correlation ID
        if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
        {
            correlationId = CorrelationIdGenerator.Generate();
        }

        // Store in HttpContext.Items for later access
        context.Items[CorrelationHeader] = correlationId;

        // Add to response headers
        context.Response.Headers[CorrelationHeader] = correlationId;

        // Continue pipeline
        await _next(context);
    }
}
