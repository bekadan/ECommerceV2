using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling.Core.Exceptions;

public abstract class ApplicationExceptionBase : Exception
{
    public string ErrorCode { get; }
    public string CorrelationId { get; }
    public SeverityLevel Severity { get; }

    protected ApplicationExceptionBase(string message, string errorCode,
        SeverityLevel severity = SeverityLevel.Error, string? correlationId = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Severity = severity;
        CorrelationId = correlationId ?? Utils.CorrelationIdGenerator.Generate();
    }
}

public enum SeverityLevel
{
    Info,
    Warning,
    Error,
    Critical
}
