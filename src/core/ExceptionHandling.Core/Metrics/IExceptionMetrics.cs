using ExceptionHandling.Core.Exceptions;

namespace ExceptionHandling.Core.Metrics;

public interface IExceptionMetrics
{
    /// <summary>
    /// Tracks an exception occurrence with severity and type.
    /// </summary>
    void Track(Exception exception, SeverityLevel severity);
}
