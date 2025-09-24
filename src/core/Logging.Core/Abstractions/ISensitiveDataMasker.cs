namespace Logging.Core.Abstractions;

public interface ISensitiveDataMasker
{
    /// <summary>
    /// Masks sensitive information in a given string (e.g., passwords, tokens, credit card numbers).
    /// </summary>
    /// <param name="input">The input string to mask.</param>
    /// <returns>The masked string.</returns>
    string Mask(string input);
}
