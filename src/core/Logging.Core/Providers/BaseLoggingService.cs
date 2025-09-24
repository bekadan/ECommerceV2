using Logging.Core.Abstractions;
using Logging.Core.Enums;
using Logging.Core.Models;

namespace Logging.Core.Providers;

public abstract class BaseLoggingService : ILoggingService
{
    private readonly ILogEnricher? _enricher;

    protected BaseLoggingService(ILogEnricher? enricher = null)
    {
        _enricher = enricher;
    }

    /// <summary>
    /// Concrete logging provider must implement this to actually send the log entry.
    /// </summary>
    /// <param name="entry">The log entry to log.</param>
    /// <returns></returns>
    public abstract Task LogAsync(LogEntry entry);

    protected LogEntry CreateEntry(LogLevel level, string message, Exception? exception = null,
                                   Dictionary<string, object?>? context = null, string? correlationId = null)
    {
        var entry = new LogEntry
        {
            Level = level,
            Message = message,
            Exception = exception,
            Context = context ?? new Dictionary<string, object?>(),
            CorrelationId = correlationId,
            Timestamp = DateTime.UtcNow
        };

        // Enrich the entry if an enricher is provided
        _enricher?.Enrich(entry);

        return entry;
    }

    // Standard log methods

    public Task LogTraceAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Trace, message, null, context, correlationId));

    public Task LogDebugAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Debug, message, null, context, correlationId));

    public Task LogInfoAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Info, message, null, context, correlationId));

    public Task LogWarningAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Warning, message, null, context, correlationId));

    public Task LogErrorAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Error, message, exception, context, correlationId));

    public Task LogCriticalAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(CreateEntry(LogLevel.Critical, message, exception, context, correlationId));
}
