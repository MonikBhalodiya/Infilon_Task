using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application;

public sealed class ApplicationRunner
{
    private readonly ISolarSystemDataService _dataService;
    private readonly IOutputService _screenOutput;
    private readonly IFileOutputService _fileOutput;

    /// <summary>
    /// Creates the application workflow coordinator.
    /// </summary>
    /// <param name="dataService">Loads and associates planet and moon data.</param>
    /// <param name="screenOutput">Writes reports to the console.</param>
    /// <param name="fileOutput">Writes reports to CSV files.</param>
    public ApplicationRunner(ISolarSystemDataService dataService, IOutputService screenOutput, IFileOutputService fileOutput)
    {
        _dataService = dataService;
        _screenOutput = screenOutput;
        _fileOutput = fileOutput;
    }

    /// <summary>
    /// Loads the data once, calculates report values, and writes all console and CSV reports.
    /// </summary>
    /// <param name="cancellationToken">Cancels API or file operations.</param>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        Logger.Instance.Info(LoggerMessage.LoadingData);
        var snapshot = await _dataService.LoadAsync(cancellationToken);

        Logger.Instance.Info(LoggerMessage.CalculatingAverages);
        _ = snapshot.Planets.Where(planet => planet.HasMoons()).Select(planet => planet.AverageMoonTemperatureKelvin).ToArray();

        Logger.Instance.Info(LoggerMessage.WritingConsole);
        _screenOutput.OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole(snapshot.Planets);
        _screenOutput.OutputAllMoonsAndTheirMassToConsole(snapshot.Moons);
        _screenOutput.OutputAllPlanetsAndTheirMoonsToConsole(snapshot.Planets);

        Logger.Instance.Info(LoggerMessage.WritingFiles);
        await _fileOutput.WriteAllReportsAsync(snapshot, cancellationToken);
        Logger.Instance.Info($"{LoggerMessage.Completed} Output directory: {_fileOutput.OutputDirectory}");
    }
}