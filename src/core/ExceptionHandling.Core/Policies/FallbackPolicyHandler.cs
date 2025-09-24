using Polly;
using Polly.Fallback;

namespace ExceptionHandling.Core.Policies;

public class FallbackPolicyHandler
{
    /// <summary>
    /// Creates a fallback policy to provide a default value when an exception occurs.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="fallbackValue"></param>
    /// <returns></returns>
    public AsyncFallbackPolicy<TResult> CreatePolicy<TResult>(TResult fallbackValue)
    {
        return Policy<TResult>
            .Handle<Exception>()
            .FallbackAsync(fallbackValue, onFallbackAsync: async (ex, context) =>
            {
                // Log fallback event
                Console.WriteLine($"Fallback triggered for {ex.Exception.GetType().Name}: {ex.Exception.Message}");
                await Task.CompletedTask;
            });
    }
}
