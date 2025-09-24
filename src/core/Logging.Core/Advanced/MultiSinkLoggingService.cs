using Logging.Core.Abstractions;
using Logging.Core.Enums;
using Logging.Core.Models;

namespace Logging.Core.Advanced;

public class MultiSinkLoggingService : ILoggingService
{
    private readonly IEnumerable<ILoggingService> _sinks;

    public MultiSinkLoggingService(IEnumerable<ILoggingService> sinks)
    {
        _sinks = sinks;
    }

    public async Task LogAsync(LogEntry entry)
    {
        foreach (var sink in _sinks)
        {
            try
            {
                await sink.LogAsync(entry);
            }
            catch
            {
                // Swallow errors for non-critical sinks
            }
        }
    }

    public Task LogTraceAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Trace, Message = message, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });

    public Task LogDebugAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Debug, Message = message, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });

    public Task LogInfoAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Info, Message = message, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });

    public Task LogWarningAsync(string message, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Warning, Message = message, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });

    public Task LogErrorAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Error, Message = message, Exception = exception, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });

    public Task LogCriticalAsync(string message, Exception exception, Dictionary<string, object?>? context = null, string? correlationId = null)
        => LogAsync(new LogEntry { Level = LogLevel.Critical, Message = message, Exception = exception, Context = context ?? new Dictionary<string, object?>(), CorrelationId = correlationId });
}
