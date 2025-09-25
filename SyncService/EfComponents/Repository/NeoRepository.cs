using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.EfComponents.DbSets;
using SyncService.Extensions;

namespace SyncService.EfComponents.Repository;

public class NeoRepository : INeoRepository
{
    private readonly NeoContext _neoContext;

    public NeoRepository(NeoContext neoContext)
    {
        _neoContext = neoContext;
    }

    public async Task SaveOrUpdate(DbSetsDto dto, CancellationToken cancellationToken)
    {
        _neoContext.SyncDateTimes.Add(dto.SyncDateTime);
        _neoContext.CloseApproachData.AddRange(dto.CloseApproachData);
        
        var dbObjectsIds = GetCurrentDbObjectsIds();
        foreach (var obj in dto.NearEarthObjects)
        {
            var id = obj.Id;
            if (dbObjectsIds.Contains(id))
            {
                var dbObj = await _neoContext.NearEarthObjects.FindAsync(id);
                dbObj?.Change(obj);
            }
            else
            {
                await _neoContext.NearEarthObjects.AddAsync(obj, cancellationToken);
            }
        }
        
        await _neoContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<DbNearEarthObject> GetFilteredQuery(NeoFilterRequest filter)
    {
        var query = _neoContext.NearEarthObjects.AsQueryable();

        if (filter.From.HasValue)
            query = query.Where(x => x.CloseApproachData.CloseApproachDate >= filter.From);
        if (filter.To.HasValue)
            query = query.Where(x => x.CloseApproachData.CloseApproachDate <= filter.To);
        if (filter.IsHazardous.HasValue)
            query = query.Where(x => x.IsPotentiallyHazardous == filter.IsHazardous);
        if (filter.MinDiameter.HasValue)
            query = query.Where(x => x.EstimatedDiameterMax >= filter.MinDiameter);
        if (filter.MaxDiameter.HasValue)
            query = query.Where(x => x.EstimatedDiameterMin <= filter.MaxDiameter);

        return query;
    }

    public IQueryable<DbNearEarthObject> GetNearEarthObjectsAsQueryable() => _neoContext.NearEarthObjects.AsQueryable();

    private HashSet<string> GetCurrentDbObjectsIds()
    {
        return _neoContext.NearEarthObjects
            .Select(x => x.Id)
            .ToHashSet();
    }
}