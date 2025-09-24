using SyncService.Api.NeoWatcher;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.NeoWatcherApi.Controllers.NeoStats;

namespace SyncService.BusinessLogic;

public interface INeoStatService
{
    Task <IEnumerable<NeoStatsResponse>> GetFilteredStatsAsync(NeoFilterRequest filter, CancellationToken cancellationToken);
}