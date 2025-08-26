using SyncService.EfComponents;
using SyncService.NeoApiComponents.Main;

namespace SyncService. Extensions;

public static class NearEarthObjectExtensions
{
    public static SimplifiedNearEarthObject Simplify(this NearEarthObject obj) =>
        new()
        {
            Id = obj.Id,
            Name = obj.Name,
            CloseApproachDate = obj.CloseApproachData.First().CloseApproachDateFull,
            EstimatedDiameterMin = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMin,
            EstimatedDiameterMax = obj.EstimatedDiameter.Kilometers.EstimatedDiameterMax,
            IsPotentiallyHazardous = obj.IsPotentiallyHazardousAsteroid,
            RelativeVelocityKmh = obj.CloseApproachData.First().RelativeVelocity.KilometersPerHour,
            MissDistanceKm = obj.CloseApproachData.First().MissDistance.Kilometers
        };
}