namespace ExceptionHandling.Core.Models;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public string CorrelationId { get; set; } = null!;
    public List<ErrorDetail>? Details { get; set; }
}
