using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

public class NeoSearchControllerTests(CustomWebApplicationFactory factory) : BaseControllersTests(factory)
{
    [Theory]
    [InlineData("http://localhost:5000/neo/search?Name=gq", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/search?Name=Non%20existent%20result", HttpStatusCode.NoContent)]
    [InlineData("http://localhost:5000/neo/search?Page=0", HttpStatusCode.BadRequest)]
    [InlineData("http://localhost:5000/neo/search?MinDiameter=10&MaxDiameter=7", HttpStatusCode.BadRequest)]
    [InlineData("http://localhost:5000/neo/search?MinDiameter=-7", HttpStatusCode.BadRequest)]
    [InlineData("http://localhost:5000/neo/search?MaxDiameter=-7", HttpStatusCode.BadRequest)]
    public async Task NeoSearchController_SearchAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
    
    [Theory]
    [InlineData("http://localhost:5000/neo/search/FindSimilar?neoId=54543395", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/search/FindSimilar?neoId=3840783", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/search/FindSimilar?neoId=50", HttpStatusCode.NoContent)]
    public async Task NeoSearchController_FindSimilarAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
    
    [Theory]
    [InlineData("http://localhost:5000/neo/search/GetSuggestions?query=gq", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/search/GetSuggestions?query=rb", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/search/GetSuggestions?query=non%20existent%20name", HttpStatusCode.NoContent)]
    public async Task NeoSearchController_GetSuggestionsAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
}