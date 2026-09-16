using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects;

public sealed class MassDto
{
    [JsonProperty("massValue")]
    public decimal? Value { get; set; }

    [JsonProperty("massExponent")]
    public int? Exponent { get; set; }
}