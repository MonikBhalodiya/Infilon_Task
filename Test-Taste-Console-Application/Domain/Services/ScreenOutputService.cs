using System.Globalization;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class ScreenOutputService : IOutputService
{
    /// <summary>
    /// Writes each planet and its associated moons as formatted console tables.
    /// </summary>
    /// <param name="planets">Planets to display.</param>
    public void OutputAllPlanetsAndTheirMoonsToConsole(IReadOnlyList<Planet> planets)
    {
        if (planets.Count == 0)
        {
            Console.WriteLine(OutputString.NoPlanetsFound);
            return;
        }

        var planetColumnSizes = new[] { 20, 20, 30, 20 };
        var planetColumnLabels = new[]
        {
            OutputString.PlanetNumber,
            OutputString.PlanetId,
            OutputString.PlanetSemiMajorAxis,
            OutputString.TotalMoons
        };
        var moonColumnSizes = new[] { 20, 72 };
        var moonColumnLabels = new[] { OutputString.MoonNumber, OutputString.MoonId };

        for (var planetIndex = 0; planetIndex < planets.Count; planetIndex++)
        {
            var planet = planets[planetIndex];
            ConsoleWriter.CreateLine(planetColumnSizes);
            ConsoleWriter.CreateText(planetColumnLabels, planetColumnSizes);
            ConsoleWriter.CreateText(
                new[]
                {
                    (planetIndex + 1).ToString(CultureInfo.InvariantCulture),
                    ToDisplayName(planet.Id),
                    planet.SemiMajorAxis.ToString(CultureInfo.InvariantCulture),
                    planet.Moons.Count.ToString(CultureInfo.InvariantCulture)
                },
                planetColumnSizes);

            ConsoleWriter.CreateLine(planetColumnSizes);
            ConsoleWriter.CreateText(moonColumnLabels, moonColumnSizes);
            var moons = planet.Moons.ToArray();
            for (var moonIndex = 0; moonIndex < moons.Length; moonIndex++)
            {
                ConsoleWriter.CreateText(
                    new[]
                    {
                        (moonIndex + 1).ToString(CultureInfo.InvariantCulture),
                        ToDisplayName(moons[moonIndex].Id)
                    },
                    moonColumnSizes);
            }

            ConsoleWriter.CreateLine(moonColumnSizes);
            ConsoleWriter.CreateEmptyLines(2);
        }
    }

    /// <summary>
    /// Writes all moons and their available mass components to the console.
    /// </summary>
    /// <param name="moons">Moons to display.</param>
    public void OutputAllMoonsAndTheirMassToConsole(IReadOnlyList<Moon> moons)
    {
        if (moons.Count == 0)
        {
            Console.WriteLine(OutputString.NoMoonsFound);
            return;
        }

        var columnSizes = new[] { 20, 25, 25, 25 };
        var columnLabels = new[]
        {
            OutputString.MoonNumber,
            OutputString.MoonId,
            OutputString.MoonMassExponent,
            OutputString.MoonMassValue
        };

        ConsoleWriter.CreateHeader(columnLabels, columnSizes);
        for (var index = 0; index < moons.Count; index++)
        {
            var moon = moons[index];
            ConsoleWriter.CreateText(
                new[]
                {
                    (index + 1).ToString(CultureInfo.InvariantCulture),
                    ToDisplayName(moon.Id),
                    moon.MassExponent?.ToString(CultureInfo.InvariantCulture) ?? OutputString.NotAvailable,
                    moon.MassValue?.ToString(CultureInfo.InvariantCulture) ?? OutputString.NotAvailable
                },
                columnSizes);
        }

        ConsoleWriter.CreateLine(columnSizes);
        ConsoleWriter.CreateEmptyLines(2);
    }

    /// <summary>
    /// Writes average moon temperatures for planets that have at least one moon.
    /// </summary>
    /// <param name="planets">Planets to evaluate and display.</param>
    public void OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole(
        IReadOnlyList<Planet> planets)
    {
        var planetsWithMoons = planets.Where(planet => planet.HasMoons()).ToArray();
        if (planetsWithMoons.Length == 0)
        {
            Console.WriteLine(OutputString.NoPlanetsFound);
            return;
        }

        var columnSizes = new[] { 25, 35 };
        var columnLabels = new[]
        {
            OutputString.PlanetId,
            OutputString.PlanetMoonAverageTemperature
        };

        ConsoleWriter.CreateHeader(columnLabels, columnSizes);
        foreach (var planet in planetsWithMoons)
        {
            var average = planet.AverageMoonTemperatureKelvin;
            ConsoleWriter.CreateText(
                new[]
                {
                    ToDisplayName(planet.Id),
                    average?.ToString("F2", CultureInfo.InvariantCulture) ?? OutputString.NotAvailable
                },
                columnSizes);
        }

        ConsoleWriter.CreateLine(columnSizes);
        ConsoleWriter.CreateEmptyLines(2);
    }

    /// <summary>
    /// Converts an API identifier to a title-cased display value.
    /// </summary>
    /// <param name="value">Identifier to format.</param>
    /// <returns>The title-cased value.</returns>
    private static string ToDisplayName(string value) => CultureInfoUtility.TextInfo.ToTitleCase(value);
}