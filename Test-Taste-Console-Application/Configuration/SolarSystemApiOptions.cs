using System.Text;
using Test_Taste_Console_Application.Constants;

namespace Test_Taste_Console_Application.Configuration;

public sealed class SolarSystemApiOptions
{
    public Uri BaseAddress { get; init; } = null!;
    public string ApiKey { get; init; } = string.Empty;
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Prompts for the API key and creates the fixed Solar System API settings.
    /// </summary>
    /// <returns>Validated API settings.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no API key is supplied.</exception>
    public static SolarSystemApiOptions Create()
    {
        var apiKey = ReadApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("The API key is required.");
        }

        return new SolarSystemApiOptions
        {
            ApiKey = apiKey.Trim(),
            BaseAddress = new Uri(UriPath.BaseUri),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    /// <summary>
    /// Reads the API key from standard input without displaying typed characters.
    /// </summary>
    /// <returns>The API key entered by the user.</returns>
    private static string? ReadApiKey()
    {
        Console.Write("Enter Solar System API key: ");

        if (Console.IsInputRedirected)
        {
            return Console.ReadLine();
        }

        var value = new StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return value.ToString();
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (value.Length > 0)
                {
                    value.Length--;
                }

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                value.Append(key.KeyChar);
            }
        }
    }
}
