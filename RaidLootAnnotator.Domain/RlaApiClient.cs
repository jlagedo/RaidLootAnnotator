using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RaidLootAnnotator.ApiClient
{
    public class RlaApiClient                               
    {
        private readonly string _staticUrl;
        private readonly HttpClient httpClient;
        private readonly ILogger<RlaApiClient> logger;
        private readonly string secretkey;

        public RlaApiClient(HttpClient httpClient, ILogger<RlaApiClient> logger)
        {
            secretkey = "";
            this.httpClient = httpClient;
            this.logger = logger;
            _staticUrl = "https://rlapi-661465551617.southamerica-east1.run.app/static";
        }

        public async Task<HttpResponseMessage> PostStaticAsync(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _staticUrl);

            request.Headers.Add("secretkey", secretkey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(new { name }),
                Encoding.UTF8,
                "application/json"
            );

            logger.LogDebug("Sending POST to {Url} with name: {Name}", _staticUrl, name);

            var response = await httpClient.SendAsync(request);

            logger.LogDebug("Received response: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);

            return response;
        }

        public async Task<StaticResponse?> PostStaticAndReadResponseAsync(string name)
        {
            const int maxRetries = 3;
            const int delayMilliseconds = 1000;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    logger.LogDebug("Attempt {Attempt}: Posting static for name '{Name}'", attempt, name);
                    var response = await PostStaticAsync(name);
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();

                    logger.LogDebug("Response JSON: {Json}", json);

                    var result = JsonSerializer.Deserialize<StaticResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (result == null)
                    {
                        logger.LogError("Failed to deserialize response.");
                        throw new InvalidOperationException("Failed to deserialize response.");
                    }
                    logger.LogInformation("Successfully deserialized StaticResponse with Guid: {Guid}", result.Guid);
                    return result;
                }
                catch (HttpRequestException ex) when (attempt < maxRetries)
                {
                    logger.LogWarning(ex, "HttpRequestException on attempt {Attempt}: {Message}. Retrying...", attempt, ex.Message);
                    await Task.Delay(delayMilliseconds);
                }
                catch (TaskCanceledException ex) when (attempt < maxRetries)
                {
                    logger.LogWarning(ex, "TaskCanceledException (timeout) on attempt {Attempt}: {Message}. Retrying...", attempt, ex.Message);
                    await Task.Delay(delayMilliseconds);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception in PostStaticAndReadResponseAsync: {Message}", ex.Message);
                    throw;
                }
            }
            logger.LogError("Failed to get a successful response after retries.");
            throw new InvalidOperationException("Failed to get a successful response after retries.");
        }
    }
}
