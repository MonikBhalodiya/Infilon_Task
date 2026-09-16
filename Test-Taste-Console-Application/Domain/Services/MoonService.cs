using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.DataTransferObjects;
using Test_Taste_Console_Application.Domain.DataTransferObjects.JsonObjects;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class MoonService : IMoonService
{
    private readonly ISolarSystemApiClient _apiClient;

    /// <summary>
    /// Creates the moon data service.
    /// </summary>
    /// <param name="apiClient">Client used to query the Solar System API.</param>
    public MoonService(ISolarSystemApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves all moons and maps their mass, temperature, and parent-planet data.
    /// </summary>
    /// <param name="cancellationToken">Cancels the API request.</param>
    /// <returns>A read-only collection of valid moons.</returns>
    public async Task<IReadOnlyList<Moon>> GetAllMoonsAsync(CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetAsync<JsonResult<MoonDto>>(
            UriPath.GetAllMoons,
            cancellationToken);

        return result.Bodies
            .Where(moon => !string.IsNullOrWhiteSpace(moon.Id))
            .Select(moon => new Moon(moon))
            .ToArray();
    }
}
