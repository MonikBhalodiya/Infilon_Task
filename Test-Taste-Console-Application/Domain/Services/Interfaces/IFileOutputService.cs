using Test_Taste_Console_Application.Domain.Objects;

namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface IFileOutputService
{
    string OutputDirectory { get; }

    /// <summary>
    /// Writes every CSV report for the supplied snapshot.
    /// </summary>
    /// <param name="snapshot">Associated planet and moon data.</param>
    /// <param name="cancellationToken">Cancels file writing.</param>
    Task WriteAllReportsAsync(
        SolarSystemSnapshot snapshot,
        CancellationToken cancellationToken);
}
