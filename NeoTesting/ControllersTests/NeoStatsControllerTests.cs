using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

public class NeoStatsControllerTests(CustomWebApplicationFactory factory) : BaseControllersTests(factory)
{
    [Theory]
    [InlineData("http://localhost:5000/neo/stats?From=09.02.25&To=09.15.2025&IsHazardous=true", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/stats?From=09.02.2009&To=09.15.2012&IsHazardous=true", HttpStatusCode.NoContent)]
    [InlineData("http://localhost:5000/neo/stats?From=09.02.2015&To=09.15.2012&IsHazardous=true", HttpStatusCode.BadRequest)]
    [InlineData("http://localhost:5000/neo/stats?MinDiameter=-7", HttpStatusCode.BadRequest)]
    [InlineData("http://localhost:5000/neo/stats?MaxDiameter=-7", HttpStatusCode.BadRequest)]
    public async Task NeoStatisticsController_GetBasicStatisticsAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
}