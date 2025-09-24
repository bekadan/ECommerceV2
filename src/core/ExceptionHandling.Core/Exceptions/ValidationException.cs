namespace ExceptionHandling.Core.Exceptions;

public class ValidationException : ApplicationExceptionBase
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors, string? correlationId = null)
        : base("Validation failed", "VALIDATION_FAILED", SeverityLevel.Warning, correlationId)
    {
        Errors = errors;
    }
}
