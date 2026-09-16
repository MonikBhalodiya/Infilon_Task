using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects;

public sealed class PlanetDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("semimajorAxis")]
    public long SemiMajorAxis { get; set; }

    [JsonProperty("moons")]
    public IReadOnlyCollection<MoonReferenceDto>? Moons { get; set; }
}