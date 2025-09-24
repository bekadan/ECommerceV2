namespace ExceptionHandling.Core.Exceptions;

public class ExternalServiceException : ApplicationExceptionBase
{
    public ExternalServiceException(string message, string? correlationId = null)
        : base(message, "EXTERNAL_SERVICE_ERROR", SeverityLevel.Critical, correlationId) { }
}
