using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects;

public sealed class AroundPlanetDto
{
    [JsonProperty("planet")]
    public string Planet { get; set; } = string.Empty;

    [JsonProperty("rel")]
    public string RelationUrl { get; set; } = string.Empty;
}