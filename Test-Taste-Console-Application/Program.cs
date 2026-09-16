using System.Net.Http.Headers;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Test_Taste_Console_Application.Configuration;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application;

internal static class Program
{
    /// <summary>
    /// Configures the application, runs the reporting workflow, and returns a process exit code.
    /// </summary>
    /// <returns>Zero on success, one on failure, or two when the user cancels the operation.</returns>
    private static async Task<int> Main()
    {
        XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()!), new FileInfo(ConfigurationFileName.Logger));
        using var cancellationSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationSource.Cancel();
        };

        try
        {
            var apiOptions = SolarSystemApiOptions.Create();
            var outputOptions = OutputOptions.CreateDefault();
            var services = new ServiceCollection();
            ConfigureServices(services, apiOptions, outputOptions);

            await using var serviceProvider = services.BuildServiceProvider();
            var runner = serviceProvider.GetRequiredService<ApplicationRunner>();
            await runner.RunAsync(cancellationSource.Token);
            return 0;
        }
        catch (OperationCanceledException) when (cancellationSource.IsCancellationRequested)
        {
            Logger.Instance.Warn(LoggerMessage.OperationCancelled);
            return 2;
        }
        catch (Exception exception)
        {
            Logger.Instance.Error(ExceptionMessage.ApplicationFailed, exception);
            return 1;
        }
    }

    /// <summary>
    /// Registers configuration, the authenticated HTTP client, and application services.
    /// </summary>
    /// <param name="services">The dependency-injection service collection.</param>
    /// <param name="apiOptions">Validated Solar System API settings.</param>
    /// <param name="outputOptions">CSV output settings.</param>
    private static void ConfigureServices(IServiceCollection services, SolarSystemApiOptions apiOptions, OutputOptions outputOptions)
    {
        services.AddSingleton(apiOptions);
        services.AddSingleton(outputOptions);

        services.AddHttpClient<ISolarSystemApiClient, SolarSystemApiClient>(client =>
        {
            client.BaseAddress = apiOptions.BaseAddress;
            client.Timeout = apiOptions.Timeout;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpClientSettings.JsonType));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiOptions.ApiKey);
        });

        services.AddTransient<IPlanetService, PlanetService>();
        services.AddTransient<IMoonService, MoonService>();
        services.AddTransient<ISolarSystemDataService, SolarSystemDataService>();
        services.AddTransient<IOutputService, ScreenOutputService>();
        services.AddTransient<IFileOutputService, FileOutputService>();
        services.AddTransient<ApplicationRunner>();
    }
}
