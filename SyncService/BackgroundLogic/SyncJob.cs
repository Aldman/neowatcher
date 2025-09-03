using SyncService.EfComponents;
using SyncService.EfComponents.DbSets;
using SyncService.Extensions;
using SyncService.Helpers;
using SyncService.NeoApiComponents.Main;

namespace SyncService.BackgroundLogic;

public class SyncJob
{
    private readonly HttpClient _httpClient;
    private readonly NeoContext _neoContext;

    public SyncJob(HttpClient httpClient, NeoContext neoContext)
    {
        _httpClient = httpClient;
        _neoContext = neoContext;
    }

    public async Task UpdateDataOfNasaAsync(CancellationToken cancellationToken = default)
    {
        var neoResponse = await GetNeoResponseByApi(cancellationToken);

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

    private async Task<NeoResponse?> GetNeoResponseByApi(CancellationToken cancellationToken)
    {
        var apiLink = NasaApiLinkBuilder.GetLinkWithOnlyEndDate(endDate: DateTime.Now);
        var response = await _httpClient.GetAsync(apiLink, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        
        return json.ToNeoResponse();
    }
    
    private HashSet<string> GetCurrentDbObjectsIds()
    {
        return _neoContext.NearEarthObjects
            .Select(x => x.Id)
            .ToHashSet();
    }
}