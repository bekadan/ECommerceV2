using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Logging.Core.Extensions;

public static class CorrelationIdExtensions
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public static string GetOrCreateCorrelationId(this HttpContext context)
    {
        StringValues correlationIdValues;

        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out correlationIdValues)
            || string.IsNullOrWhiteSpace(correlationIdValues.FirstOrDefault()))
        {
            var newId = Guid.NewGuid().ToString();
            context.Request.Headers[CorrelationIdHeader] = newId;
            context.Response.Headers[CorrelationIdHeader] = newId;
            return newId;
        }

        var correlationId = correlationIdValues.FirstOrDefault()!;
        context.Response.Headers[CorrelationIdHeader] = correlationId;
        return correlationId;
    }
}
