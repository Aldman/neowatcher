using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.EfComponents.DbSets;

namespace SyncService.EfComponents.Repository;

public interface INeoRepository
{
    Task SaveOrUpdate(DbSetsDto dto, CancellationToken cancellationToken);
    IQueryable<DbNearEarthObject> GetFilteredQuery(NeoFilterRequest filter);
}