using System.Globalization;
using System.Text;
using Test_Taste_Console_Application.Configuration;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class FileOutputService : IFileOutputService
{
    public string OutputDirectory { get; }

    /// <summary>
    /// Creates the CSV output service with a resolved output directory.
    /// </summary>
    /// <param name="options">CSV output settings.</param>
    public FileOutputService(OutputOptions options)
    {
        OutputDirectory = options.DirectoryPath;
    }

    /// <summary>
    /// Writes the moon-mass, planet-moon, and average-temperature CSV reports.
    /// </summary>
    /// <param name="snapshot">Associated planet and moon data.</param>
    /// <param name="cancellationToken">Cancels file writing.</param>
    public async Task WriteAllReportsAsync(
        SolarSystemSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(OutputDirectory);

        await WriteCsvAtomicallyAsync(
            PathName.AllMoonsAndTheirMassFile,
            CreateMoonMassLines(snapshot.Moons),
            cancellationToken);
        await WriteCsvAtomicallyAsync(
            PathName.AllPlanetsAndTheirMoonsFile,
            CreatePlanetMoonLines(snapshot.Planets),
            cancellationToken);
        await WriteCsvAtomicallyAsync(
            PathName.AllPlanetsAndTheirAverageMoonTemperatureFile,
            CreateAverageTemperatureLines(snapshot.Planets),
            cancellationToken);
    }

    /// <summary>
    /// Creates CSV rows for the moon mass report.
    /// </summary>
    /// <param name="moons">Moons to serialize.</param>
    /// <returns>CSV header and data rows.</returns>
    private static IEnumerable<string> CreateMoonMassLines(IReadOnlyList<Moon> moons)
    {
        yield return ToCsvRow("MoonNumber", "MoonId", "MassExponent", "MassValue");
        for (var index = 0; index < moons.Count; index++)
        {
            var moon = moons[index];
            yield return ToCsvRow(
                (index + 1).ToString(CultureInfo.InvariantCulture),
                moon.Id,
                moon.MassExponent?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                moon.MassValue?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
        }
    }

    /// <summary>
    /// Creates flattened CSV rows for planets and their moons.
    /// </summary>
    /// <param name="planets">Planets to serialize.</param>
    /// <returns>CSV header and data rows.</returns>
    private static IEnumerable<string> CreatePlanetMoonLines(IReadOnlyList<Planet> planets)
    {
        yield return ToCsvRow(
            "PlanetNumber",
            "PlanetId",
            "SemiMajorAxis",
            "TotalMoons",
            "MoonNumber",
            "MoonId");

        for (var planetIndex = 0; planetIndex < planets.Count; planetIndex++)
        {
            var planet = planets[planetIndex];
            var moons = planet.Moons.ToArray();
            if (moons.Length == 0)
            {
                yield return ToCsvRow(
                    (planetIndex + 1).ToString(CultureInfo.InvariantCulture),
                    planet.Id,
                    planet.SemiMajorAxis.ToString(CultureInfo.InvariantCulture),
                    "0",
                    string.Empty,
                    string.Empty);
                continue;
            }

            for (var moonIndex = 0; moonIndex < moons.Length; moonIndex++)
            {
                yield return ToCsvRow(
                    (planetIndex + 1).ToString(CultureInfo.InvariantCulture),
                    planet.Id,
                    planet.SemiMajorAxis.ToString(CultureInfo.InvariantCulture),
                    moons.Length.ToString(CultureInfo.InvariantCulture),
                    (moonIndex + 1).ToString(CultureInfo.InvariantCulture),
                    moons[moonIndex].Id);
            }
        }
    }

    /// <summary>
    /// Creates average-temperature CSV rows for planets that have moons.
    /// </summary>
    /// <param name="planets">Planets to evaluate.</param>
    /// <returns>CSV header and data rows.</returns>
    private static IEnumerable<string> CreateAverageTemperatureLines(IReadOnlyList<Planet> planets)
    {
        yield return ToCsvRow("PlanetId", "AverageMoonTemperatureKelvin");
        foreach (var planet in planets.Where(planet => planet.HasMoons()))
        {
            yield return ToCsvRow(
                planet.Id,
                planet.AverageMoonTemperatureKelvin?.ToString("F2", CultureInfo.InvariantCulture)
                ?? OutputString.NotAvailable);
        }
    }

    /// <summary>
    /// Writes a CSV to a temporary file and atomically replaces the destination.
    /// </summary>
    /// <param name="fileName">Destination filename within the output directory.</param>
    /// <param name="lines">CSV lines to write.</param>
    /// <param name="cancellationToken">Cancels file writing.</param>
    private async Task WriteCsvAtomicallyAsync(
        string fileName,
        IEnumerable<string> lines,
        CancellationToken cancellationToken)
    {
        var destinationPath = Path.Combine(OutputDirectory, fileName);
        var temporaryPath = destinationPath + "." + Guid.NewGuid().ToString("N") + ".tmp";

        try
        {
            await using (var writer = new StreamWriter(
                             temporaryPath,
                             false,
                             new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)))
            {
                foreach (var line in lines)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await writer.WriteLineAsync(line.AsMemory(), cancellationToken);
                }
            }

            File.Move(temporaryPath, destinationPath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    /// <summary>
    /// Escapes individual values and combines them into one CSV row.
    /// </summary>
    /// <param name="fields">Values in column order.</param>
    /// <returns>A valid comma-separated row.</returns>
    private static string ToCsvRow(params string[] fields) =>
        string.Join(",", fields.Select(EscapeCsvField));

    /// <summary>
    /// Quotes and escapes a value when required by CSV rules.
    /// </summary>
    /// <param name="value">Raw field value.</param>
    /// <returns>The CSV-safe field value.</returns>
    private static string EscapeCsvField(string value)
    {
        if (!value.Contains(',') && !value.Contains('\"') &&
            !value.Contains('\r') && !value.Contains('\n'))
        {
            return value;
        }

        return '\"' + value.Replace("\"", "\"\"") + '\"';
    }
}
