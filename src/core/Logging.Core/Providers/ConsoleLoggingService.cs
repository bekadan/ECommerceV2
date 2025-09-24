using Logging.Core.Abstractions;
using Logging.Core.Models;

namespace Logging.Core.Providers;

public class ConsoleLoggingService : BaseLoggingService
{
    private readonly ISensitiveDataMasker? _masker;

    public ConsoleLoggingService(ILogEnricher? enricher = null, ISensitiveDataMasker? masker = null)
        : base(enricher)
    {
        _masker = masker;
    }

    public override Task LogAsync(LogEntry entry)
    {
        if (_masker != null)
        {
            entry.Message = _masker.Mask(entry.Message);
            foreach (var key in entry.Context.Keys.ToList())
            {
                if (entry.Context[key] is string str)
                    entry.Context[key] = _masker.Mask(str);
            }
        }

        Console.WriteLine($"{entry.Timestamp:u} [{entry.Level}] {entry.Message}");
        if (entry.Exception != null)
            Console.WriteLine(entry.Exception);
        return Task.CompletedTask;
    }
}
