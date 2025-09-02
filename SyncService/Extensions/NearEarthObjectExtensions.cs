using SyncService.EfComponents.DbSets;
using SyncService.NeoApiComponents.Main;

namespace SyncService. Extensions;

public static class NearEarthObjectExtensions
{
    public static SyncDateTimes PrepareFullLinkedDataForDb(this NearEarthObject obj)
    {
        var dbNearEarthObject = obj.ToDbNearEarthObject();

        var toReturn = new SyncDateTimes
        {
            Id = Guid.NewGuid(),
            SyncTime = DateTime.Now,
            NearEarthObjectId = dbNearEarthObject.Id,
            NearEarthObject = dbNearEarthObject
        };
        dbNearEarthObject.SyncDateTimeId =  toReturn.Id;
        dbNearEarthObject.SyncDateTime = toReturn;
        
        return toReturn;
    }
    
    public static DbNearEarthObject ToDbNearEarthObject(this NearEarthObject obj)
    {
        var closeApproachData = obj.ToDbCloseApproachData();
        
        var toReturn = new DbNearEarthObject
        {
            Id = obj.Id,
            Name = obj.Name,
            CloseApproachDataId = closeApproachData.Id,
            EstimatedDiameterMin = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMin,
            EstimatedDiameterMax = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMax,
            IsPotentiallyHazardous = obj.IsPotentiallyHazardousAsteroid,
            CloseApproachData = closeApproachData
        };
        closeApproachData.NearEarthObjectId = toReturn.Id;
        closeApproachData.NearEarthObject = toReturn;
        
        return toReturn;
    }
    
    private static DbCloseApproachData ToDbCloseApproachData(this NearEarthObject obj) =>
        new()
        {
            Id = Guid.NewGuid(),
            CloseApproachDate = obj.CloseApproachData.First().CloseApproachDateFull,
            EpochDateCloseApproach = obj.CloseApproachData.First().EpochDateCloseApproach,
            RelativeVelocityKmh = obj.CloseApproachData.First().RelativeVelocity.KilometersPerHour,
            MissDistanceKm = obj.CloseApproachData.First().MissDistance.Kilometers,
            OrbitingBody = obj.CloseApproachData.First().OrbitingBody
        };
}