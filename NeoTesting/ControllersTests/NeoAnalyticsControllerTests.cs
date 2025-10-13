using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

public class NeoAnalyticsControllerTests(CustomWebApplicationFactory factory) : BaseControllersTests(factory)
{
    [Theory]
    [InlineData("http://localhost:5000/neo/analytics/byDateRange?from=09.02.25&to=09.07.25", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/analytics/byDateRange?from=09.02.2009&to=09.07.2010", HttpStatusCode.NoContent)]
    [InlineData("http://localhost:5000/neo/analytics/byDateRange?from=09.02.2010&to=09.07.2009", HttpStatusCode.RequestedRangeNotSatisfiable)]
    public async Task NeoAnalyticsController_GetAnalyticsByDateRangeAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }

    [Theory]
    [InlineData("http://localhost:5000/neo/analytics/hazardousAnalysis", HttpStatusCode.OK)]
    public async Task NeoAnalyticsController_GetHazardousAnalysisAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }

    [Theory]
    [InlineData("http://localhost:5000/neo/analytics/comparePeriods?period1Start=09.02.25&period1End=09.15.25&period2Start=09.16.25&period2End=10.03.25", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/analytics/comparePeriods?period1Start=10.02.25&period1End=09.15.25&period2Start=09.16.25&period2End=10.03.25", HttpStatusCode.RequestedRangeNotSatisfiable)]
    [InlineData("http://localhost:5000/neo/analytics/comparePeriods?period1Start=09.02.2009&period1End=09.15.2009&period2Start=09.16.2010&period2End=10.03.2010", HttpStatusCode.NoContent)]
    public async Task NeoAnalyticsController_ComparePeriodsAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
}