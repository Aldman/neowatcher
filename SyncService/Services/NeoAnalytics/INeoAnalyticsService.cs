using SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

namespace SyncService.Services.NeoAnalytics;

public interface INeoAnalyticsService
{
    Task <IEnumerable<NeoAnalyticsResponse>> GetDateRangeAnalyticsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);

    public Task<IEnumerable<NeoHazardousAnalysisResponse>> GetHazardousAnalysisAsync(CancellationToken cancellationToken);
    
    Task<NeoComparisonResponse> ComparePeriodsAsync(
        DateTime period1Start,
        DateTime period1End, 
        DateTime period2Start, 
        DateTime period2End, 
        CancellationToken cancellationToken = default);
}