using SyncService.DTOs.NeoStatistics;

namespace SyncService.Services.NeoStatistics;

public interface INeoStatisticsService
{
    Task<NeoBasicStatsResponse> GetBasicStatisticsAsync(DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
}