using Logging.Core.Abstractions;
using Logging.Core.Models;
using Logging.Core.Providers;
using System.Collections.Concurrent;

namespace Logging.Core.Advanced;

public abstract class BufferedLoggingService : BaseLoggingService
{
    private readonly ConcurrentQueue<LogEntry> _buffer = new();
    private readonly Timer _flushTimer;
    private readonly int _batchSize;
    private readonly int _flushIntervalMs;

    protected BufferedLoggingService(ILogEnricher? enricher = null, int batchSize = 10, int flushIntervalMs = 5000)
        : base(enricher)
    {
        _batchSize = batchSize;
        _flushIntervalMs = flushIntervalMs;
        _flushTimer = new Timer(async _ => await FlushAsync(), null, _flushIntervalMs, _flushIntervalMs);
    }

    public override Task LogAsync(LogEntry entry)
    {
        _buffer.Enqueue(entry);

        if (_buffer.Count >= _batchSize)
            return FlushAsync();

        return Task.CompletedTask;
    }

    protected async Task FlushAsync()
    {
        var entriesToSend = new List<LogEntry>();
        while (_buffer.TryDequeue(out var entry))
        {
            entriesToSend.Add(entry);
            if (entriesToSend.Count >= _batchSize) break;
        }

        if (entriesToSend.Count == 0) return;

        int retryCount = 0;
        bool success = false;
        while (!success && retryCount < 3)
        {
            try
            {
                await SendBatchAsync(entriesToSend);
                success = true;
            }
            catch
            {
                retryCount++;
                await Task.Delay((int)Math.Pow(2, retryCount) * 1000);
            }
        }
    }

    /// <summary>
    /// Concrete class must implement this to send a batch of logs to the provider.
    /// </summary>
    protected abstract Task SendBatchAsync(IEnumerable<LogEntry> entries);
}
