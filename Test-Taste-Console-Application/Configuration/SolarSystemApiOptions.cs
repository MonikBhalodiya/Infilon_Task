namespace Test_Taste_Console_Application.Configuration;

public sealed class SolarSystemApiOptions
{
    public const string ApiKeyValue = "6536460a-76af-415f-a65e-3454a27a94e5";
    public const string BaseAddressValue = "https://api.le-systeme-solaire.net/rest/";
    public const int TimeoutSeconds = 30;

    public Uri BaseAddress { get; init; } = null!;
    public string ApiKey { get; init; } = string.Empty;
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(TimeoutSeconds);

    /// <summary>
    /// Creates API settings from the key, base address, and timeout defined in this class.
    /// </summary>
    /// <returns>The configured Solar System API settings.</returns>
    public static SolarSystemApiOptions Create()
    {
        return new SolarSystemApiOptions
        {
            ApiKey = ApiKeyValue,
            BaseAddress = new Uri(BaseAddressValue),
            Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
        };
    }
}
