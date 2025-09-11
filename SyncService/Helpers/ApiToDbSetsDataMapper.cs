using SyncService.EfComponents.DbSets;
using SyncService.Extensions;
using SyncService.NeoApiComponents.Main;

namespace SyncService.Helpers;

public static class ApiToDbSetsDataMapper
{
    public static DbSetsDto MapAndLink(NeoResponse apiData)
    {
        var syncDateEntity = new SyncDateTimes();
        var nearEarthObjects = new List<DbNearEarthObject>();
        var closeApproachData = new List<DbCloseApproachData>();
        
        var actualResponseObjects = apiData.NearEarthObjects[DateTime.Today];
        foreach (var apiObj in actualResponseObjects)
        {
            var dbObj = apiObj.ToDbNearEarthObject();
            nearEarthObjects.Add(dbObj);
            closeApproachData.Add(dbObj.CloseApproachData);
        }

        return new DbSetsDto
        {
            SyncDateTime = syncDateEntity,
            NearEarthObjects = nearEarthObjects,
            CloseApproachData = closeApproachData
        };
    }
}