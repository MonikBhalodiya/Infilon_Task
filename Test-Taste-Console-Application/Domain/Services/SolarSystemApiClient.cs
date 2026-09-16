using System.Net;
using Newtonsoft.Json;
using Test_Taste_Console_Application.Domain.Services.Interfaces;

namespace Test_Taste_Console_Application.Domain.Services;

public sealed class SolarSystemApiClient : ISolarSystemApiClient
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates an API client using the HttpClient configured by dependency injection.
    /// </summary>
    /// <param name="httpClient">The configured HTTP transport.</param>
    public SolarSystemApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Sends an asynchronous GET request and deserializes its JSON body.
    /// </summary>
    /// <typeparam name="T">Expected response model type.</typeparam>
    /// <param name="requestUri">URI relative to the configured API base address.</param>
    /// <param name="cancellationToken">Cancels the HTTP operation.</param>
    /// <returns>The deserialized response model.</returns>
    /// <exception cref="SolarSystemApiException">Thrown for HTTP, empty-body, or JSON failures.</exception>
    public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
        where T : class
    {
        using var response = await _httpClient.GetAsync(
            requestUri,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new SolarSystemApiException(
                requestUri,
                response.StatusCode,
                CreateSafeResponsePreview(content));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new SolarSystemApiException(requestUri, response.StatusCode, "The response body was empty.");
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(content)
                   ?? throw new JsonSerializationException("The response deserialized to null.");
        }
        catch (JsonException exception)
        {
            throw new SolarSystemApiException(
                requestUri,
                response.StatusCode,
                "The API returned invalid JSON.",
                exception);
        }
    }

    /// <summary>
    /// Produces a short, single-line response excerpt suitable for error messages.
    /// </summary>
    /// <param name="content">Raw response content.</param>
    /// <returns>A normalized response excerpt limited to 300 characters.</returns>
    private static string CreateSafeResponsePreview(string content)
    {
        const int maximumLength = 300;
        var normalized = content.Replace("\r", " ").Replace("\n", " ").Trim();
        return normalized.Length <= maximumLength
            ? normalized
            : normalized[..maximumLength] + "...";
    }
}

public sealed class SolarSystemApiException : Exception
{
    public string RequestUri { get; }
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Creates an exception containing endpoint and HTTP-status context.
    /// </summary>
    /// <param name="requestUri">The failed relative request URI.</param>
    /// <param name="statusCode">HTTP status returned by the API.</param>
    /// <param name="details">Safe diagnostic details.</param>
    /// <param name="innerException">Optional underlying exception.</param>
    public SolarSystemApiException(
        string requestUri,
        HttpStatusCode statusCode,
        string details,
        Exception? innerException = null)
        : base(
            $"Solar System API request '{requestUri}' failed with status {(int)statusCode} ({statusCode}). {details}",
            innerException)
    {
        RequestUri = requestUri;
        StatusCode = statusCode;
    }
}
