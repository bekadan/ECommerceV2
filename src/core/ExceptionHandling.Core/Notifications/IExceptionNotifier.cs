using ExceptionHandling.Core.Exceptions;

namespace ExceptionHandling.Core.Notifications;

public interface IExceptionNotifier
{
    /// <summary>
    /// Sends a notification for a critical exception or event.
    /// </summary>
    Task NotifyAsync(string message, string correlationId, SeverityLevel severity);
}
