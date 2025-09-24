using Polly;
using Polly.Retry;

namespace ExceptionHandling.Core.Policies;

public class RetryPolicyHandler
{
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _retryDelay;

    public RetryPolicyHandler(int maxRetryAttempts = 3, TimeSpan? retryDelay = null)
    {
        _maxRetryAttempts = maxRetryAttempts;
        _retryDelay = retryDelay ?? TimeSpan.FromSeconds(2);
    }

    /// <summary>
    /// Creates a retry policy for transient exceptions (e.g., HttpRequestException, TimeoutException).
    /// </summary>
    public AsyncRetryPolicy CreatePolicy()
    {
        return Policy
            .Handle<Exception>(IsTransient)
            .WaitAndRetryAsync(_maxRetryAttempts, attempt => _retryDelay,
                (exception, timeSpan, retryCount, context) =>
                {
                    // Log retry attempt here if needed
                    Console.WriteLine($"Retry {retryCount} for {exception.GetType().Name}, waiting {timeSpan.TotalSeconds}s");
                });
    }

    private bool IsTransient(Exception ex)
    {
        return ex is HttpRequestException || ex is TimeoutException;
    }
}
