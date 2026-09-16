using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects;

public sealed class MoonDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("mass")]
    public MassDto? Mass { get; set; }

    [JsonProperty("avgTemp")]
    public int? AverageTemperatureKelvin { get; set; }

    [JsonProperty("aroundPlanet")]
    public AroundPlanetDto? AroundPlanet { get; set; }
}