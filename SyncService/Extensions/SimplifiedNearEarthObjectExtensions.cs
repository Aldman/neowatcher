using SyncService.EfComponents;

namespace SyncService.Extensions;

public static class SimplifiedNearEarthObjectExtensions
{
    public static void Change(this SimplifiedNearEarthObject current, SimplifiedNearEarthObject other)
    {
        current.Name = other.Name;
        current.CloseApproachDate = other.CloseApproachDate;
        current.EstimatedDiameterMin = other.EstimatedDiameterMin;
        current.EstimatedDiameterMax = other.EstimatedDiameterMax;
        current.IsPotentiallyHazardous = other.IsPotentiallyHazardous;
        current.RelativeVelocityKmh = other.RelativeVelocityKmh;
        current.MissDistanceKm = other.MissDistanceKm;
        current.CreatedAt = other.CreatedAt;
    }
}