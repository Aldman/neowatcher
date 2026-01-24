using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

public class NeoStatisticsControllerTests(CustomWebApplicationFactory factory) : BaseControllersTests(factory)
{
    [Theory]
    [InlineData("http://localhost:5000/neo/statistics/basic?from=09.07.25&to=09.30.2025", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/statistics/basic?from=09.07.2009&to=09.30.2010", HttpStatusCode.NoContent)]
    [InlineData("http://localhost:5000/neo/statistics/basic?from=09.07.2011&to=09.30.2010", HttpStatusCode.RequestedRangeNotSatisfiable)]
    public async Task NeoStatisticsController_GetBasicStatisticsAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
    
    [Theory]
    [InlineData("http://localhost:5000/neo/statistics/DiameterDistribution", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/statistics/DiameterDistribution?buckets=0", HttpStatusCode.UnprocessableContent)]
    public async Task NeoStatisticsController_GetDiameterDistributionAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }

    [Theory]
    [InlineData("http://localhost:5000/neo/statistics/DetectAnomalies", HttpStatusCode.OK)]
    public async Task NeoStatisticsController_DetectAnomaliesAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
}