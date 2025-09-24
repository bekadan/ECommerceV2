using ExceptionHandling.Core.Metrics;
using ExceptionHandling.Core.Notifications;
using ExceptionHandling.Core.Options;
using ExceptionHandling.Core.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace ExceptionHandling.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionHandlingCore(
        this IServiceCollection services,
        Action<ExceptionHandlingOptions>? configureOptions = null)
    {
        var options = new ExceptionHandlingOptions();
        configureOptions?.Invoke(options);

        // Logging
        if (options.LoggerType != null)
            services.AddSingleton(typeof(IExceptionLogger), options.LoggerType);
        else
            services.AddSingleton<IExceptionLogger, DefaultExceptionLogger>();

        // Notifier (optional)
        if (options.NotifierType != null)
            services.AddSingleton(typeof(IExceptionNotifier), options.NotifierType);

        // Metrics (optional)
        if (options.MetricsType != null)
            services.AddSingleton(typeof(IExceptionMetrics), options.MetricsType);

        // Retry/Fallback
        services.AddSingleton<RetryPolicyHandler>();
        services.AddSingleton<FallbackPolicyHandler>();

        return services;
    }
}
