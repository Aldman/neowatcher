using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.NeoWatcherApi.Controllers.NeoStats;

namespace SyncService.Services.NeoStats;

public interface INeoStatsService
{
    Task <IEnumerable<NeoStatsResponse>> GetFilteredStatsAsync(NeoFilterRequest filter, CancellationToken cancellationToken);
}