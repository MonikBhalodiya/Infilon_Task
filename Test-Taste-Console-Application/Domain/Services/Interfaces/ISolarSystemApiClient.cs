namespace Test_Taste_Console_Application.Domain.Services.Interfaces;

public interface ISolarSystemApiClient
{
    /// <summary>
    /// Sends a GET request and deserializes the JSON response.
    /// </summary>
    /// <typeparam name="T">Expected response model type.</typeparam>
    /// <param name="requestUri">URI relative to the API base address.</param>
    /// <param name="cancellationToken">Cancels the request.</param>
    /// <returns>The deserialized response model.</returns>
    Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
        where T : class;
}
