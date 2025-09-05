using SyncService.EfComponents;
using SyncService.EfComponents.DbSets;
using SyncService.Extensions;
using SyncService.Helpers;
using SyncService.NeoApiComponents.Main;

namespace SyncService.BackgroundLogic;

public class SyncJob
{
    private readonly INasaApiClient _apiClient;
    private readonly NeoContext _neoContext;

    public SyncJob(INasaApiClient apiClient, NeoContext neoContext)
    {
        _apiClient = apiClient;
        _neoContext = neoContext;
    }

    public async Task UpdateDataOfNasaAsync(CancellationToken cancellationToken = default)
    {
        var neoResponse = await _apiClient.GetNeoResponseByApi(cancellationToken);

        var actualResponseObjects = neoResponse.NearEarthObjects[DateTime.Today];
        var dbObjectsIds = GetCurrentDbObjectsIds();
        var syncDateEntity = new SyncDateTimes();

        foreach (var obj in actualResponseObjects)
        {
            var id = obj.Id;
            var objForDb = obj.ToDbNearEarthObject();
            syncDateEntity.NearEarthObjects.Add(objForDb);
            
            if (dbObjectsIds.Contains(id))
            {
                var dbObj = await _neoContext.NearEarthObjects.FindAsync(id);
                dbObj?.Change(objForDb);
            }
            else
            {
                await _neoContext.NearEarthObjects.AddAsync(objForDb, cancellationToken);
            }

            await _neoContext.CloseApproachData.AddAsync(objForDb.CloseApproachData, cancellationToken);
        }

        await _neoContext.SyncDateTimes.AddAsync(syncDateEntity, cancellationToken);
        await _neoContext.SaveChangesAsync(cancellationToken);
    }
    
    private HashSet<string> GetCurrentDbObjectsIds()
    {
        return _neoContext.NearEarthObjects
            .Select(x => x.Id)
            .ToHashSet();
    }
}