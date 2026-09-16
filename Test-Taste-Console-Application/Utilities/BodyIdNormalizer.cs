using System.Globalization;
using System.Text;

namespace Test_Taste_Console_Application.Utilities;

public static class BodyIdNormalizer
{
    /// <summary>
    /// Normalizes a body identifier for case-insensitive and diacritic-insensitive matching.
    /// </summary>
    /// <param name="value">The API identifier to normalize.</param>
    /// <returns>A trimmed, lowercase identifier without combining diacritical marks.</returns>
    public static string Normalize(string value)
    {
        var normalized = value.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                builder.Append(char.ToLowerInvariant(character));
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}