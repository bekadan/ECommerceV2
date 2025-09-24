using ExceptionHandling.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ExceptionHandling.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionHandlingCore(this IApplicationBuilder app)
    {
        // Correlation ID must be first
        app.UseCorrelationId();

        // Exception handling middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
