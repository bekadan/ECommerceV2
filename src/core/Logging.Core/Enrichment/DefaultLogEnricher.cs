using Logging.Core.Abstractions;
using Logging.Core.Models;

namespace Logging.Core.Enrichment;

public class DefaultLogEnricher : ILogEnricher
{
    private readonly string _environment;
    private readonly string _applicationName;

    public DefaultLogEnricher(string environment, string applicationName)
    {
        _environment = environment;
        _applicationName = applicationName;
    }

    public void Enrich(LogEntry entry)
    {
        entry.Context["Environment"] = _environment;
        entry.Context["Application"] = _applicationName;
        entry.Context["Host"] = Environment.MachineName;

        if (string.IsNullOrEmpty(entry.CorrelationId))
            entry.CorrelationId = Guid.NewGuid().ToString();

        entry.Context["CorrelationId"] = entry.CorrelationId;
    }
}
