using Test_Taste_Console_Application.Domain.Objects;

namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface IPlanetService
{
    /// <summary>
    /// Retrieves all planets required by the reports.
    /// </summary>
    /// <param name="cancellationToken">Cancels the API request.</param>
    /// <returns>A read-only planet collection.</returns>
    Task<IReadOnlyList<Planet>> GetAllPlanetsAsync(CancellationToken cancellationToken);
}
