using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.DataTransferObjects;
using Test_Taste_Console_Application.Domain.DataTransferObjects.JsonObjects;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class PlanetService : IPlanetService
{
    private readonly ISolarSystemApiClient _apiClient;

    /// <summary>
    /// Creates the planet data service.
    /// </summary>
    /// <param name="apiClient">Client used to query the Solar System API.</param>
    public PlanetService(ISolarSystemApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves all planets and maps their identifiers and orbital data.
    /// </summary>
    /// <param name="cancellationToken">Cancels the API request.</param>
    /// <returns>A read-only collection of valid planets.</returns>
    public async Task<IReadOnlyList<Planet>> GetAllPlanetsAsync(CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetAsync<JsonResult<PlanetDto>>(
            UriPath.GetAllPlanets,
            cancellationToken);

        return result.Bodies
            .Where(planet => !string.IsNullOrWhiteSpace(planet.Id))
            .Select(planet => new Planet(planet))
            .ToArray();
    }
}
