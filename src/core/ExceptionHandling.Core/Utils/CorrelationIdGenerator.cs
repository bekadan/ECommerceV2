namespace ExceptionHandling.Core.Utils;

public static class CorrelationIdGenerator
{
    public static string Generate() => Guid.NewGuid().ToString("N");
}
