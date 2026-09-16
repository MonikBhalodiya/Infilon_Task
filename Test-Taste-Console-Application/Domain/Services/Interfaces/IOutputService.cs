using Test_Taste_Console_Application.Domain.Objects;

namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface IOutputService
{
    /// <summary>
    /// Writes the planet and moon relationship report to the console.
    /// </summary>
    /// <param name="planets">Planets to display.</param>
    void OutputAllPlanetsAndTheirMoonsToConsole(IReadOnlyList<Planet> planets);

    /// <summary>
    /// Writes the moon mass report to the console.
    /// </summary>
    /// <param name="moons">Moons to display.</param>
    void OutputAllMoonsAndTheirMassToConsole(IReadOnlyList<Moon> moons);

    /// <summary>
    /// Writes average moon temperatures for planets with moons.
    /// </summary>
    /// <param name="planets">Planets to evaluate and display.</param>
    void OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole(IReadOnlyList<Planet> planets);
}
