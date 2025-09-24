namespace ExceptionHandling.Core.Exceptions;

public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(string message, string? correlationId = null)
        : base(message, "NOT_FOUND", SeverityLevel.Warning, correlationId) { }
}
