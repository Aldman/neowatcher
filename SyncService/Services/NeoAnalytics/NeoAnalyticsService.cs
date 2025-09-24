using SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

namespace SyncService.Services.NeoAnalytics;

public class NeoAnalyticsService : INeoAnalyticsService
{
    public Task<IEnumerable<NeoAnalyticsResponse>> GetDateRangeAnalyticsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}