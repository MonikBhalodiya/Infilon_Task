using Test_Taste_Console_Application.Domain.DataTransferObjects;

namespace Test_Taste_Console_Application.Domain.Objects;

public sealed class Planet
{
    public string Id { get; }
    public long SemiMajorAxis { get; }
    public IReadOnlyCollection<Moon> Moons { get; }

    /// <summary>
    /// Gets the arithmetic mean of known moon temperatures, or null when none are known.
    /// </summary>
    public double? AverageMoonTemperatureKelvin {
        get
        {
            var knownTemperatures = Moons.Where(moon => moon.AverageTemperatureKelvin.HasValue).Select(moon => moon.AverageTemperatureKelvin!.Value).ToArray();
            return knownTemperatures.Length == 0 ? null : knownTemperatures.Average();
        }
    }

    /// <summary>
    /// Creates an unassociated planet from an API transfer object.
    /// </summary>
    /// <param name="planetDto">Deserialized planet data.</param>
    public Planet(PlanetDto planetDto) : this(planetDto.Id, planetDto.SemiMajorAxis, Array.Empty<Moon>())
    {
    }

    /// <summary>
    /// Creates a planet with its associated moons.
    /// </summary>
    /// <param name="id">Planet identifier.</param>
    /// <param name="semiMajorAxis">Orbital semi-major axis in kilometres.</param>
    /// <param name="moons">Moons associated with the planet.</param>
    public Planet(string id, long semiMajorAxis, IEnumerable<Moon> moons)
    {
        Id = id;
        SemiMajorAxis = semiMajorAxis;
        Moons = moons.ToArray();
    }

    /// <summary>
    /// Determines whether the planet has at least one associated moon.
    /// </summary>
    /// <returns>True when one or more moons are associated.</returns>
    public bool HasMoons() => Moons.Count > 0;

    /// <summary>
    /// Creates a copy of the planet with a replacement moon collection.
    /// </summary>
    /// <param name="moons">Moons to associate with the new planet instance.</param>
    /// <returns>A new planet containing the supplied moons.</returns>
    public Planet WithMoons(IEnumerable<Moon> moons) => new(Id, SemiMajorAxis, moons);
}