using Test_Taste_Console_Application.Domain.Objects;

namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface ISolarSystemDataService
{
    /// <summary>
    /// Loads and combines the planet and moon datasets.
    /// </summary>
    /// <param name="cancellationToken">Cancels data loading.</param>
    /// <returns>An associated solar-system snapshot.</returns>
    Task<SolarSystemSnapshot> LoadAsync(CancellationToken cancellationToken);
}
