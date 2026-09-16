using Test_Taste_Console_Application.Domain.Objects;

namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface IMoonService
{
    /// <summary>
    /// Retrieves all moons required by the reports.
    /// </summary>
    /// <param name="cancellationToken">Cancels the API request.</param>
    /// <returns>A read-only moon collection.</returns>
    Task<IReadOnlyList<Moon>> GetAllMoonsAsync(CancellationToken cancellationToken);
}
