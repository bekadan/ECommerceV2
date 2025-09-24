using Logging.Core.Abstractions;
using Logging.Core.Enums;
using Logging.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Logging.Core.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _logger;

    public LoggingMiddleware(RequestDelegate next, ILoggingService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;

        context.Request.Body.Position = 0;
        string requestBody = "";
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        await _next(context);

        var logEntry = new LogEntry
        {
            Level = LogLevel.Info,
            Message = $"{context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode}",
            CorrelationId = correlationId,
            Context = new Dictionary<string, object?> { ["RequestBody"] = requestBody, ["ResponseStatusCode"] = context.Response.StatusCode }
        };

        await _logger.LogAsync(logEntry);
    }
}
