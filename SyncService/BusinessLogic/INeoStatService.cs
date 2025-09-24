using SyncService.Api.NeoWatcher;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;

namespace SyncService.BusinessLogic;

public interface INeoStatService
{
    Task <IEnumerable<NeoStatResponse>> GetFilteredStatsAsync(NeoFilterRequest filter, CancellationToken cancellationToken);
}