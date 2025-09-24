using Logging.Core.Models;

namespace Logging.Core.Abstractions;

public interface ILoggingService
{
    Task LogAsync(LogEntry entry);
    Task LogTraceAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null);
    Task LogDebugAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null);
    Task LogInfoAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null);
    Task LogWarningAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null);
    Task LogErrorAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null);
    Task LogCriticalAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null);
}
