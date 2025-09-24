using Amazon.CloudWatchLogs;
using Logging.Core.Abstractions;
using Logging.Core.Enrichment;
using Logging.Core.Enums;
using Logging.Core.Masking;
using Logging.Core.Providers;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using LoggingOptions = Logging.Core.Options.LoggingOptions;

namespace Logging.Core.Extensions;

public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddLoggingService(this IServiceCollection services, LoggingOptions options)
    {
        // Add enrichment and masking
        services.AddSingleton<ILogEnricher>(new DefaultLogEnricher(options.Environment, options.ApplicationName));
        services.AddSingleton<ISensitiveDataMasker, DefaultSensitiveDataMasker>();

        // Register the logging provider
        switch (options.ProviderType)
        {
            case LoggingProviderType.Console:
                services.AddSingleton<ILoggingService, ConsoleLoggingService>();
                break;

            case LoggingProviderType.GCP:
                if (string.IsNullOrWhiteSpace(options.GcpProjectId))
                    throw new ArgumentNullException(nameof(options.GcpProjectId), "GCP Project ID must be provided for GCP logging.");

                services.AddSingleton<ILoggingService>(sp =>
                    new GcpLoggingService(options.GcpProjectId!, options.LogName, sp.GetService<ILogEnricher>()));
                break;

            case LoggingProviderType.Azure:
                // TelemetryClient must be registered in ASP.NET Core
                services.AddSingleton<ILoggingService>(sp =>
                {
                    var telemetryClient = sp.GetRequiredService<TelemetryClient>();
                    return new AzureLoggingService(telemetryClient, sp.GetService<ILogEnricher>());
                });
                break;

            case LoggingProviderType.AWS:
                if (string.IsNullOrWhiteSpace(options.LogGroupName))
                    throw new ArgumentNullException(nameof(options.LogGroupName), "AWS LogGroupName must be provided for AWS logging.");
                if (string.IsNullOrWhiteSpace(options.LogStreamName))
                    throw new ArgumentNullException(nameof(options.LogStreamName), "AWS LogStreamName must be provided for AWS logging.");

                services.AddAWSService<IAmazonCloudWatchLogs>(); // Ensure AWS SDK DI
                services.AddSingleton<ILoggingService>(sp =>
                {
                    var client = sp.GetRequiredService<IAmazonCloudWatchLogs>();
                    return new AwsLoggingService(client, options.LogGroupName!, options.LogStreamName!, sp.GetService<ILogEnricher>());
                });
                break;

            default:
                throw new NotSupportedException($"Logging provider '{options.ProviderType}' is not supported.");
        }

        return services;
    }
}
