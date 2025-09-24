using Logging.Core.Models;

namespace Logging.Core.Abstractions;

public interface ILogEnricher
{
    /// <summary>
    /// Enriches a log entry with additional context information.
    /// </summary>
    /// <param name="entry">The log entry to enrich.</param>
    void Enrich(LogEntry entry);
}
