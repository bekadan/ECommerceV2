using ExceptionHandling.Core.Exceptions;
using System.Text;
using System.Text.Json;

namespace ExceptionHandling.Core.Notifications;

public class SlackNotifier : IExceptionNotifier
{
    private readonly string _webhookUrl;
    private readonly HttpClient _httpClient;

    public SlackNotifier(string webhookUrl)
    {
        _webhookUrl = webhookUrl;
        _httpClient = new HttpClient();
    }

    public async Task NotifyAsync(string message, string correlationId, SeverityLevel severity)
    {
        var payload = new
        {
            text = $"[{severity}] CorrelationId: {correlationId} - {message}"
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await _httpClient.PostAsync(_webhookUrl, content);
    }
}
