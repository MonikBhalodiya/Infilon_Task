namespace Test_Taste_Console_Application.Domain.Objects;

/// <summary>
/// Represents the complete, associated planet and moon dataset used by all reports.
/// </summary>
/// <param name="Planets">Planets with their associated moon collections.</param>
/// <param name="Moons">All moons returned by the API.</param>
public sealed record SolarSystemSnapshot(IReadOnlyList<Planet> Planets, IReadOnlyList<Moon> Moons);