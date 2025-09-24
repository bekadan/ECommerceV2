using Logging.Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using System.Text;

namespace Logging.Core.Extensions
{
    public static class HttpRequestLoggingExtensions
    {
        public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app, ILoggingService loggingService)
        {
            return app.Use(async (context, next) =>
            {
                var request = context.Request;
                request.Body.Position = 0;

                string requestBody = string.Empty;
                if (request.ContentLength > 0)
                {
                    using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }

                await loggingService.LogInfoAsync($"HTTP Request: {request.Method} {request.Path}", new Dictionary<string, object?>
            {
                { "RequestBody", requestBody },
                { "Headers", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()) }
            });

                // Capture response
                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await next();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await loggingService.LogInfoAsync($"HTTP Response: {context.Response.StatusCode}", new Dictionary<string, object?>
            {
                { "ResponseBody", responseText }
            });

                await responseBody.CopyToAsync(originalBodyStream);
            });
        }
    }
}
