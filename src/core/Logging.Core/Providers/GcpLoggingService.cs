using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Logging.Core.Abstractions;
using Logging.Core.Enums;

namespace Logging.Core.Providers;

public class GcpLoggingService : BaseLoggingService
{
    private readonly LoggingServiceV2Client _client;
    private readonly string _projectId;
    private readonly string _logName;

    public GcpLoggingService(string projectId, string logName = "app-logs", ILogEnricher? enricher = null)
        : base(enricher)
    {
        _projectId = projectId;
        _logName = logName;
        _client = LoggingServiceV2Client.Create();
    }

    public override async Task LogAsync(Models.LogEntry entry)
    {
        var gcpEntry = new LogEntry
        {
            LogName = $"projects/{_projectId}/logs/{_logName}",
            Severity = MapSeverity(entry.Level),
            TextPayload = entry.Message,
            Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(entry.Timestamp.ToUniversalTime())
        };

        // Add context properties as labels
        foreach (var kvp in entry.Context)
        {
            gcpEntry.Labels[kvp.Key] = kvp.Value?.ToString() ?? "";
        }

        var resource = new MonitoredResource { Type = "global" };

        await _client.WriteLogEntriesAsync(
            logName: $"projects/{_projectId}/logs/{_logName}", // string format
            resource: resource,
            labels: gcpEntry.Labels.ToDictionary(k => k.Key, k => k.Value), // ensure IDictionary<string, string>
            entries: new[] { gcpEntry } // must be IEnumerable<LogEntry>
        );
    }

    private LogSeverity MapSeverity(LogLevel level) => level switch
    {
        LogLevel.Trace => LogSeverity.Debug,
        LogLevel.Debug => LogSeverity.Debug,
        LogLevel.Info => LogSeverity.Info,
        LogLevel.Warning => LogSeverity.Warning,
        LogLevel.Error => LogSeverity.Error,
        LogLevel.Critical => LogSeverity.Critical,
        _ => LogSeverity.Default
    };
}
