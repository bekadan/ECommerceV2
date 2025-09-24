using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Logging.Core.Abstractions;
using Logging.Core.Models;

namespace Logging.Core.Providers;

public class AwsLoggingService : BaseLoggingService
{
    private readonly IAmazonCloudWatchLogs _client;
    private readonly string _logGroupName;
    private readonly string _logStreamName;

    public AwsLoggingService(
        IAmazonCloudWatchLogs client,
        string logGroupName,
        string logStreamName,
        ILogEnricher? enricher = null)
        : base(enricher)
    {
        _client = client;
        _logGroupName = logGroupName;
        _logStreamName = logStreamName;
    }

    public override async Task LogAsync(LogEntry entry)
    {
        // Ensure the log stream exists
        await EnsureLogStreamAsync();

        var message = entry.Exception != null
            ? $"{entry.Level}: {entry.Message}\n{entry.Exception}"
            : $"{entry.Level}: {entry.Message}";

        var inputLogEvent = new InputLogEvent
        {
            Timestamp = entry.Timestamp,
            Message = message
        };

        var request = new PutLogEventsRequest
        {
            LogGroupName = _logGroupName,
            LogStreamName = _logStreamName,
            LogEvents = new List<InputLogEvent> { inputLogEvent }
        };

        // Get the next sequence token
        var tokenResponse = await _client.DescribeLogStreamsAsync(new DescribeLogStreamsRequest
        {
            LogGroupName = _logGroupName,
            LogStreamNamePrefix = _logStreamName
        });

        var logStream = tokenResponse.LogStreams.FirstOrDefault();
        if (logStream?.UploadSequenceToken != null)
            request.SequenceToken = logStream.UploadSequenceToken;

        await _client.PutLogEventsAsync(request);
    }

    private async Task EnsureLogStreamAsync()
    {
        try
        {
            // Ensure log group exists
            await _client.CreateLogGroupAsync(new CreateLogGroupRequest { LogGroupName = _logGroupName });
        }
        catch (ResourceAlreadyExistsException) { }

        try
        {
            // Ensure log stream exists
            await _client.CreateLogStreamAsync(new CreateLogStreamRequest
            {
                LogGroupName = _logGroupName,
                LogStreamName = _logStreamName
            });
        }
        catch (ResourceAlreadyExistsException) { }
    }
}
