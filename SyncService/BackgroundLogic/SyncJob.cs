using SyncService.EfComponents;
using SyncService.EfComponents.Contexts;
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
        await RemoveExtraObjectsFromDb(actualResponseObjects, dbObjectsIds);

        foreach (var obj in actualResponseObjects)
        {
            var id = obj.Id;
            var objForDb = obj.ToDbNearEarthObject();
            
            if (!dbObjectsIds.Contains(id))
                await _neoContext.NearEarthObjects.AddAsync(objForDb, cancellationToken);
            
            var dbObj = await _neoContext.NearEarthObjects.FindAsync(id);
            dbObj?.Change(objForDb);
        }

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

    private async Task RemoveExtraObjectsFromDb(List<NearEarthObject> actualResponseObjects, HashSet<string> dbObjectsIds)
    {
        var responseIds = actualResponseObjects
            .Select(x => x.Id)
            .ToHashSet();

        var extraDbIds = dbObjectsIds.Except(responseIds);
        foreach (var extraDbId in extraDbIds)
        {
            var extra = await _neoContext.NearEarthObjects.FindAsync(extraDbId);
            if (extra != null)
                _neoContext.NearEarthObjects.Remove(extra);
        }
    }
}