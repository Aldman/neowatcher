using SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

namespace SyncService.Services.NeoAnalytics;

public interface INeoAnalyticsService
{
    Task <IEnumerable<NeoAnalyticsResponse>> GetDateRangeAnalyticsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);
}