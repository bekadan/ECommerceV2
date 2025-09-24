using ExceptionHandling.Core.Exceptions;
using ExceptionHandling.Core.Metrics;
using ExceptionHandling.Core.Notifications;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ExceptionHandling.Core.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionLogger _logger;
    private readonly IExceptionNotifier? _notifier;
    private readonly IExceptionMetrics? _metrics;

    public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionLogger logger, IExceptionNotifier? notifier = null, IExceptionMetrics? metrics = null)
    {
        _next = next;
        _logger = logger;
        _notifier = notifier;
        _metrics = metrics;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        const string CorrelationHeader = "X-Correlation-ID";

        // Get correlation ID from HttpContext.Items
        var correlationId = context.Items[CorrelationHeader]?.ToString() ?? Guid.NewGuid().ToString("N");

        var response = new Models.ErrorResponse
        {
            Message = exception.Message,
            CorrelationId = correlationId
        };

        int statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ExternalServiceException => StatusCodes.Status503ServiceUnavailable,
            InternalServerException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        response.StatusCode = statusCode;

        // 1️⃣ Log the exception
        _logger.Log(exception, correlationId);

        // 2️⃣ Track metrics
        _metrics?.Track(exception, exception is ApplicationExceptionBase appEx ? appEx.Severity : SeverityLevel.Error);

        // 3️⃣ Notify if critical
        if (exception is ApplicationExceptionBase appEx2 && appEx2.Severity == SeverityLevel.Critical)
        {
            if (_notifier != null)
                await _notifier.NotifyAsync(appEx2.Message, correlationId, appEx2.Severity);
        }

        // 4️⃣ Return standardized response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
