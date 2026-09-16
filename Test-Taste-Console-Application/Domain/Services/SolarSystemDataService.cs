using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class SolarSystemDataService : ISolarSystemDataService
{
    private readonly IPlanetService _planetService;
    private readonly IMoonService _moonService;

    /// <summary>
    /// Creates the service that combines planet and moon datasets.
    /// </summary>
    /// <param name="planetService">Loads planet data.</param>
    /// <param name="moonService">Loads moon data.</param>
    public SolarSystemDataService(IPlanetService planetService, IMoonService moonService)
    {
        _planetService = planetService;
        _moonService = moonService;
    }

    /// <summary>
    /// Loads planets and moons concurrently and associates moons with parent planets.
    /// </summary>
    /// <param name="cancellationToken">Cancels either API request.</param>
    /// <returns>A fully associated snapshot for all reports.</returns>
    public async Task<SolarSystemSnapshot> LoadAsync(CancellationToken cancellationToken)
    {
        var planetsTask = _planetService.GetAllPlanetsAsync(cancellationToken);
        var moonsTask = _moonService.GetAllMoonsAsync(cancellationToken);
        await Task.WhenAll(planetsTask, moonsTask);

        var planets = await planetsTask;
        var moons = await moonsTask;
        var moonsByPlanet = moons
            .Where(moon => !string.IsNullOrWhiteSpace(moon.ParentPlanetId))
            .GroupBy(
                moon => BodyIdNormalizer.Normalize(moon.ParentPlanetId!),
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.OrdinalIgnoreCase);

        var hydratedPlanets = planets
            .Select(planet =>
            {
                var key = BodyIdNormalizer.Normalize(planet.Id);
                return planet.WithMoons(
                    moonsByPlanet.TryGetValue(key, out var planetMoons)
                        ? planetMoons
                        : Array.Empty<Moon>());
            })
            .ToArray();

        return new SolarSystemSnapshot(hydratedPlanets, moons);
    }
}
