using Logging.Core.Models;

namespace Logging.Core.Abstractions;

public interface ILogAlertHandler
{
    /// <summary>
    /// Handles a log entry that requires alerting (usually critical errors).
    /// </summary>
    /// <param name="entry">The log entry that triggered the alert.</param>
    /// <returns>A task representing the async operation.</returns>
    Task HandleAlertAsync(LogEntry entry);
}
