using System.Net;
using NeoTesting.Auxiliary;

namespace NeoTesting.ControllersTests;

public class NeoReportingControllerTests(CustomWebApplicationFactory factory) : BaseControllersTests(factory)
{
    [Theory]
    [InlineData("http://localhost:5000/neo/reporting/DailyReport?date=09.05.2025", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/reporting/DailyReport?date=09.05.2024", HttpStatusCode.NoContent)]
    public async Task NeoReportingController_GetDailyReportAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
    
    [Theory]
    [InlineData("http://localhost:5000/neo/reporting/WeeklyReport?week=09.15.2025", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/reporting/WeeklyReport?week=09.15.2015", HttpStatusCode.NoContent)]
    public async Task NeoReportingController_GetWeeklyReportAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
    
    [Theory]
    [InlineData("http://localhost:5000/neo/reporting/SummaryReport?from=09.07.2025&to=09.10.2025", HttpStatusCode.OK)]
    [InlineData("http://localhost:5000/neo/reporting/SummaryReport?from=09.07.2015&to=09.10.2015", HttpStatusCode.NoContent)]
    public async Task NeoReportingController_GetSummaryReportAsync_ShouldReturnExpectedStatusCode(
        string url, HttpStatusCode expectedStatusCode, CancellationToken ct = default)
    {
        await AssertStatusCodes(url, expectedStatusCode, ct);
    }
}