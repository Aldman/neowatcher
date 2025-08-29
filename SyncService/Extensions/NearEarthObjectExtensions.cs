using SyncService.EfComponents;
using SyncService.EfComponents.DbSets;
using SyncService.NeoApiComponents.Main;

namespace SyncService. Extensions;

public static class NearEarthObjectExtensions
{
    public static DbNearEarthObject ToDbNearEarthObject(this NearEarthObject obj)
    {
        var closeApproachDataId = Guid.NewGuid();
        
        return new DbNearEarthObject
        {
            Id = obj.Id,
            Name = obj.Name,
            CloseApproachDataId = closeApproachDataId,
            EstimatedDiameterMin = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMin,
            EstimatedDiameterMax = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMax,
            IsPotentiallyHazardous = obj.IsPotentiallyHazardousAsteroid,
            CloseApproachData = new DbCloseApproachData
            {
                Id = closeApproachDataId,
                CloseApproachDate = obj.CloseApproachData.First().CloseApproachDateFull,
                EpochDateCloseApproach = obj.CloseApproachData.First().EpochDateCloseApproach,
                RelativeVelocityKmh = obj.CloseApproachData.First().RelativeVelocity.KilometersPerHour,
                MissDistanceKm = obj.CloseApproachData.First().MissDistance.Kilometers,
                OrbitingBody = obj.CloseApproachData.First().OrbitingBody,
            }
        };
    }
}