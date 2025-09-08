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
    
    private HashSet<string> GetCurrentDbObjectsIds()
    {
        return _neoContext.NearEarthObjects
            .Select(x => x.Id)
            .ToHashSet();
    }
}