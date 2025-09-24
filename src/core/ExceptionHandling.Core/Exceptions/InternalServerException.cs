namespace ExceptionHandling.Core.Exceptions;

/// <summary>
/// Represents an unhandled server-side exception.
/// Typically maps to HTTP 500.
/// </summary>
public class InternalServerException : ApplicationExceptionBase
{
    public InternalServerException(string message = "An internal server error occurred",
        string? correlationId = null)
        : base(message, "INTERNAL_SERVER_ERROR", SeverityLevel.Critical, correlationId)
    {
    }

    public InternalServerException(string message, Exception innerException, string? correlationId = null)
        : base(message, "INTERNAL_SERVER_ERROR", SeverityLevel.Critical, correlationId)
    {
        // Optionally wrap the inner exception
        this.InnerException = innerException;
    }
}
