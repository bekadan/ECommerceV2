using Logging.Core.Enums;

namespace Logging.Core.Options;

public class LoggingOptions
{
    /// <summary>
    /// The logging provider to use (Console, GCP, Azure, AWS).
    /// </summary>
    public LoggingProviderType ProviderType { get; set; } = LoggingProviderType.Console;

    /// <summary>
    /// Application environment (Development, Staging, Production).
    /// </summary>
    public string Environment { get; set; } = "Development";

    /// <summary>
    /// Application name, used for enrichment.
    /// </summary>
    public string ApplicationName { get; set; } = "App";

    /// <summary>
    /// Optional: log name for cloud providers.
    /// </summary>
    public string LogName { get; set; } = "app-logs";

    /// <summary>
    /// GCP project ID (required if ProviderType = GCP).
    /// </summary>
    public string? GcpProjectId { get; set; }

    /// <summary>
    /// AWS CloudWatch log group name (required if ProviderType = AWS).
    /// </summary>
    public string? LogGroupName { get; set; }

    /// <summary>
    /// AWS CloudWatch log stream name (required if ProviderType = AWS).
    /// </summary>
    public string? LogStreamName { get; set; }

    /// <summary>
    /// Optional: enable logging request/response bodies in middleware.
    /// </summary>
    public bool LogHttpBodies { get; set; } = false;

    /// <summary>
    /// Optional: batch size for buffered logging.
    /// </summary>
    public int BatchSize { get; set; } = 10;

    /// <summary>
    /// Optional: flush interval in milliseconds for buffered logging.
    /// </summary>
    public int FlushIntervalMs { get; set; } = 5000;
}
