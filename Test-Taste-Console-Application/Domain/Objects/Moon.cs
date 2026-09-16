using Test_Taste_Console_Application.Domain.DataTransferObjects;

namespace Test_Taste_Console_Application.Domain.Objects;

public sealed class Moon
{
    public string Id { get; }
    public decimal? MassValue { get; }
    public int? MassExponent { get; }
    public int? AverageTemperatureKelvin { get; }
    public string? ParentPlanetId { get; }

    /// <summary>
    /// Creates a moon domain object from an API transfer object.
    /// </summary>
    /// <param name="moonDto">Deserialized moon data.</param>
    public Moon(MoonDto moonDto)
    {
        if (moonDto == null)
            throw new ArgumentNullException(nameof(moonDto));

        Id = moonDto.Id;
        MassValue = moonDto.Mass?.Value;
        MassExponent = moonDto.Mass?.Exponent;
        AverageTemperatureKelvin = moonDto.AverageTemperatureKelvin;
        ParentPlanetId = string.IsNullOrWhiteSpace(moonDto.AroundPlanet?.Planet) ? null : moonDto.AroundPlanet.Planet;
    }

    /// <summary>
    /// Creates a moon from explicit values, primarily for domain composition.
    /// </summary>
    /// <param name="id">Moon identifier.</param>
    /// <param name="averageTemperatureKelvin">Mean temperature in Kelvin, when known.</param>
    /// <param name="parentPlanetId">Parent planet identifier, when known.</param>
    /// <param name="massValue">Numeric mass component, when known.</param>
    /// <param name="massExponent">Base-10 mass exponent, when known.</param>
    public Moon(string id, int? averageTemperatureKelvin, string? parentPlanetId = null, decimal? massValue = null, int? massExponent = null)
    {
        Id = id;
        AverageTemperatureKelvin = averageTemperatureKelvin;
        ParentPlanetId = parentPlanetId;
        MassValue = massValue;
        MassExponent = massExponent;
    }
}