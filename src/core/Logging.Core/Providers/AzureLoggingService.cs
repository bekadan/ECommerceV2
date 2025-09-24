using Logging.Core.Abstractions;
using Logging.Core.Enums;
using Logging.Core.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Logging.Core.Providers;

public class AzureLoggingService : BaseLoggingService
{
    private readonly TelemetryClient _telemetryClient;

    public AzureLoggingService(TelemetryClient telemetryClient, ILogEnricher? enricher = null)
        : base(enricher)
    {
        _telemetryClient = telemetryClient;
    }

    public override Task LogAsync(LogEntry entry)
    {
        var severity = MapSeverity(entry.Level);

        // Track exceptions
        if (entry.Exception != null)
        {
            var exceptionTelemetry = new ExceptionTelemetry(entry.Exception)
            {
                SeverityLevel = severity,
                Message = entry.Message
            };

            // Add context properties
            foreach (var kvp in entry.Context)
                exceptionTelemetry.Properties[kvp.Key] = kvp.Value?.ToString() ?? "";

            _telemetryClient.TrackException(exceptionTelemetry);
        }
        else
        {
            var traceTelemetry = new TraceTelemetry(entry.Message, severity);

            // Add context properties
            foreach (var kvp in entry.Context)
                traceTelemetry.Properties[kvp.Key] = kvp.Value?.ToString() ?? "";

            _telemetryClient.TrackTrace(traceTelemetry);
        }

        return Task.CompletedTask;
    }

    private static SeverityLevel MapSeverity(LogLevel level) => level switch
    {
        LogLevel.Trace => SeverityLevel.Verbose,
        LogLevel.Debug => SeverityLevel.Verbose,
        LogLevel.Info => SeverityLevel.Information,
        LogLevel.Warning => SeverityLevel.Warning,
        LogLevel.Error => SeverityLevel.Error,
        LogLevel.Critical => SeverityLevel.Critical,
        _ => SeverityLevel.Information
    };
}
