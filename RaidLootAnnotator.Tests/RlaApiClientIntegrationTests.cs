using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaidLootAnnotator.ApiClient;
using Xunit;

namespace RaidLootAnnotator.Tests
{
  public class RlaApiClientIntegrationTests
  {
    [Fact]
    public async Task PostStaticAndReadResponseAsync_ReturnsValidGuid()
    {
      // Arrange
      var httpClient = new HttpClient();

      var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<RlaApiClient>.Instance;
      var client = new RlaApiClient(httpClient, logger); // Pass the logger
      // Use a unique name to avoid conflicts
      var testName = $"TestGroup_{Guid.NewGuid()}";

      // Act
      var response = await client.PostStaticAndReadResponseAsync(testName);

      // Assert
      Assert.NotNull(response);
      Assert.NotEqual(Guid.Empty, response!.Guid);
    }
  }
}
