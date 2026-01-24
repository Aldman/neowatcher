using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

[Collection(WellKnownConstants.ApplicationFactory)]
public class BaseControllersTests
{
    private readonly CustomWebApplicationFactory _factory;

    protected BaseControllersTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    protected async Task AssertStatusCodes(string url, HttpStatusCode expected, CancellationToken ct = default)
    {
        // Arrange
        var client = _factory.HttpClient;

        // Act
        var response = await client.GetAsync(url, ct);
        
        // Assert
        Assert.Equal(expected, response.StatusCode);
    }
}