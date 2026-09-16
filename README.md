# Solar System Console Application

A .NET 5 console application that retrieves planet and moon information from the Solar System OpenData API, calculates the average temperature of each planet's moons, prints formatted reports, and writes the same information to CSV files.

## Features

- Loads planets and moons with two concurrent bulk API requests.
- Associates each moon with its parent planet.
- Calculates the average known moon temperature for planets that have moons.
- Lists all planets and their moons.
- Lists all moons and their mass values.
- Writes three CSV reports using temporary files and atomic replacement.
- Supports cancellation with Ctrl+C.
- Logs loading, calculation, console output, file output, completion, and failure states.
- Keeps the API key outside source code.

## Requirements

- .NET 5 runtime or a newer SDK capable of targeting .NET 5.
- A free Solar System OpenData API key.
- Internet access when running the application.

Important: .NET 5 is end-of-life and no longer receives security updates. Building with a current SDK produces warning NETSDK1138.

## Project structure

    Test-Taste-Console-Application/
    |-- ApplicationRunner.cs
    |-- Program.cs
    |-- Configuration/
    |   |-- OutputOptions.cs
    |   |-- SolarSystemApiOptions.cs
    |-- Constants/
    |   |-- ConfigurationFileName.cs
    |   |-- ExceptionMessage.cs
    |   |-- HttpClientSettings.cs
    |   |-- LoggerMessage.cs
    |   |-- OutputString.cs
    |   |-- PathName.cs
    |   |-- UriPath.cs
    |-- Domain/
    |   |-- DataTransferObjects/
    |   |   |-- AroundPlanetDto.cs
    |   |   |-- MassDto.cs
    |   |   |-- MoonDto.cs
    |   |   |-- MoonReferenceDto.cs
    |   |   |-- PlanetDto.cs
    |   |   |-- JsonObjects/JsonResult.cs
    |   |-- Objects/
    |   |   |-- Moon.cs
    |   |   |-- Planet.cs
    |   |   |-- SolarSystemSnapshot.cs
    |   |-- Services/
    |       |-- FileOutputService.cs
    |       |-- MoonService.cs
    |       |-- PlanetService.cs
    |       |-- ScreenOutputService.cs
    |       |-- SolarSystemApiClient.cs
    |       |-- SolarSystemDataService.cs
    |       |-- Interfaces/
    |-- Utilities/
    |   |-- BodyIdNormalizer.cs
    |   |-- ConsoleWriter.cs
    |   |-- CultureInfoUtility.cs
    |   |-- Logger.cs
    |-- log4net.config

## Architecture and responsibilities

| Component | Responsibility |
| --- | --- |
| Program | Validates configuration, configures dependency injection and HttpClient, handles cancellation, and returns the process exit code. |
| ApplicationRunner | Coordinates loading, calculation, console output, and CSV output. |
| SolarSystemApiClient | Performs authenticated asynchronous GET requests and deserializes JSON responses. |
| PlanetService | Loads and maps planet data. |
| MoonService | Loads and maps moon, mass, temperature, and parent-planet data. |
| SolarSystemDataService | Runs both bulk requests concurrently and associates moons with planets. |
| Planet and Moon | Hold domain data; Planet calculates the average known moon temperature. |
| ScreenOutputService | Produces formatted console tables. |
| FileOutputService | Produces the three CSV reports. |

## API information

Base URL:

    https://api.le-systeme-solaire.net/rest/

Authentication is required for every request:

    Authorization: Bearer <API key>
    Accept: application/json

### Endpoints used

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | /bodies | Retrieves filtered planet or moon collections. |

Planet request:

    GET /bodies?data=id,semimajorAxis,moons&filter[]=isPlanet,eq,true

Moon request:

    GET /bodies?data=id,mass,massValue,massExponent,avgTemp,aroundPlanet,planet,rel&filter[]=bodyType,eq,Moon

### API fields used

| Field | Description |
| --- | --- |
| id | API identifier for a planet or moon. |
| semimajorAxis | Orbital semi-major axis in kilometres. |
| moons | Moon references belonging to a planet. |
| mass.massValue | Numeric portion of a moon's mass. |
| mass.massExponent | Base-10 exponent of a moon's mass. |
| avgTemp | Mean temperature in Kelvin. |
| aroundPlanet.planet | Identifier of the moon's parent planet. |
| aroundPlanet.rel | API relation URL for the parent planet. |

The current API may return zero for moon temperatures. The application preserves zero as an API-provided numeric value. Missing temperature properties remain unknown and are excluded from the average; a planet whose moons all have missing temperatures displays N/A.

## Configuration

The application does not use an appsettings.json file or environment variables. When the application starts, it securely prompts for the API key without displaying the entered characters. The API base URL and 30-second timeout are defined by SolarSystemApiOptions.

The CSV path is configured in Configuration/OutputOptions.cs:

    public const string OutputDirectoryPath = "./Files/";

The path is resolved against the process working directory. When the application is run from this repository root, output is written to:

    D:\Infilon_Task\Files

## Execution flow

1. Read and validate the API configuration.
2. Configure dependency injection and the authenticated typed HttpClient.
3. Request planets and moons concurrently.
4. Deserialize API DTOs and map them to domain objects.
5. Normalize planet identifiers and associate moons with their parent planets.
6. Calculate average known moon temperatures.
7. Print the three reports to the console.
8. Write the three CSV files.
9. Log the output location and return exit code 0.

Exit code 1 indicates an application/configuration/API/file failure. Exit code 2 indicates user cancellation.

## Generated reports

The Files directory contains:

- AllPlanetsAndTheirMoons.csv
- AllMoonsAndTheirMass.csv
- AllPlanetsAndTheirAverageMoonTemperature.csv

CSV files use UTF-8 without a byte-order mark and invariant-culture numeric formatting. Each report is first written to a temporary file and then moved over the destination file to avoid leaving partially written output.

## Build and run

Build:

    dotnet build .\Test-Taste-Console-Application.sln --configuration Release

Run from the repository root:

    dotnet run --project .\Test-Taste-Console-Application\Test-Taste-Console-Application.csproj --configuration Release

Enter the API key when prompted. Press Ctrl+C to cancel a running operation.

## Error handling

- Missing or invalid configuration stops the application before any request.
- Non-successful HTTP responses include endpoint and status information.
- Invalid or empty JSON responses produce contextual API exceptions.
- Authentication credentials are never written to logs.
- CSV writes use temporary files and clean them up after failure.
- Exceptions are logged centrally and result in a nonzero exit code.

## Application run flow

1. The user starts the console application with the dotnet run command.
2. Program.Main configures log4net and registers Ctrl+C cancellation handling.
3. SolarSystemApiOptions securely prompts the user for the API key and validates it.
4. SolarSystemApiOptions creates the fixed API base URL and 30-second timeout settings.
5. OutputOptions resolves the ./Files/ path against the current working directory.
6. Program configures dependency injection and creates the typed HttpClient.
7. The HttpClient receives the API base address, timeout, JSON Accept header, and Bearer authorization header.
8. ApplicationRunner logs the Loading solar-system data message.
9. SolarSystemDataService starts the planet request and moon request concurrently.
10. PlanetService requests all planets using the isPlanet filter.
11. MoonService requests all moons together with their mass, temperature, and parent-planet information.
12. SolarSystemApiClient validates each HTTP response and deserializes the JSON into DTO objects.
13. PlanetService and MoonService convert the DTO objects into Planet and Moon domain objects.
14. SolarSystemDataService normalizes planet identifiers and associates every moon with its parent planet.
15. A SolarSystemSnapshot containing the complete associated dataset is returned to ApplicationRunner.
16. Each Planet calculates the arithmetic average of its moons' known temperature values.
17. ScreenOutputService prints the average-temperature, moon-mass, and planet-moon reports.
18. FileOutputService creates the Files directory when it does not already exist.
19. FileOutputService creates the three CSV reports using invariant numeric formatting and CSV escaping.
20. Each CSV is written to a temporary file and then moved over its final destination to prevent partial output.
21. ApplicationRunner logs the output directory and successful completion.
22. Program returns exit code 0 to the operating system.

If an API, configuration, JSON, or file error occurs, the exception is logged and Program returns exit code 1. If the user presses Ctrl+C, pending work is cancelled and Program returns exit code 2.