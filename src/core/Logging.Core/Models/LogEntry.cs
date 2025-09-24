using Logging.Core.Enums;

namespace Logging.Core.Models;

public class LogEntry
{
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? CorrelationId { get; set; }
    public Dictionary<string, object?> Context { get; set; } = new();
}
