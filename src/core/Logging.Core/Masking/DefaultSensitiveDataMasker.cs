using Logging.Core.Abstractions;
using System.Text.RegularExpressions;

namespace Logging.Core.Masking;

public class DefaultSensitiveDataMasker : ISensitiveDataMasker
{
    private readonly string[] _sensitiveKeys = new[] { "password", "token", "creditCard" };

    public string Mask(string input)
    {
        foreach (var key in _sensitiveKeys)
            input = Regex.Replace(input, $"(\"{key}\"\\s*:\\s*\").*?\"", $"$1*****\"", RegexOptions.IgnoreCase);

        return input;
    }
}
