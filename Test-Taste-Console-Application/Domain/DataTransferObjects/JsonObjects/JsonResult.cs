using Newtonsoft.Json;

namespace Test_Taste_Console_Application.Domain.DataTransferObjects.JsonObjects;

public sealed class JsonResult<T>
{
    [JsonProperty("bodies")]
    public IReadOnlyCollection<T> Bodies { get; set; } = Array.Empty<T>();
}