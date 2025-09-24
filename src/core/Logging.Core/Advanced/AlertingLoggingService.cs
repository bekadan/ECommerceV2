using Logging.Core.Abstractions;
using Logging.Core.Enums;
using Logging.Core.Models;
using Logging.Core.Providers;

namespace Logging.Core.Advanced;

public abstract class AlertingLoggingService : BaseLoggingService
{
    private readonly IEnumerable<ILogAlertHandler> _alertHandlers;

    protected AlertingLoggingService(IEnumerable<ILogAlertHandler> alertHandlers, ILogEnricher? enricher = null)
        : base(enricher)
    {
        _alertHandlers = alertHandlers;
    }

    public override async Task LogAsync(LogEntry entry)
    {
        await HandleLoggingAsync(entry);

        if (entry.Level >= LogLevel.Critical)
        {
            foreach (var handler in _alertHandlers)
            {
                await handler.HandleAlertAsync(entry);
            }
        }
    }

    /// <summary>
    /// Concrete class must implement this to handle actual logging
    /// </summary>
    protected abstract Task HandleLoggingAsync(LogEntry entry);
}
