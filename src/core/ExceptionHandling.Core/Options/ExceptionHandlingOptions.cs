namespace ExceptionHandling.Core.Options;

public class ExceptionHandlingOptions
{
    public Type? LoggerType { get; set; }          // Custom logger
    public Type? NotifierType { get; set; }        // Custom notifier
    public Type? MetricsType { get; set; }         // Custom metrics
}
