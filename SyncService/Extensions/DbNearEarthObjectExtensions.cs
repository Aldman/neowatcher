using SyncService.EfComponents.DbSets;

namespace SyncService.Extensions;

public static class DbNearEarthObjectExtensions
{
    public static void Change(this DbNearEarthObject current, DbNearEarthObject other)
    {
        current.Name = other.Name;
        current.EstimatedDiameterMin = other.EstimatedDiameterMin;
        current.EstimatedDiameterMax = other.EstimatedDiameterMax;
        current.IsPotentiallyHazardous = other.IsPotentiallyHazardous;
        current.CloseApproachData = other.CloseApproachData;

        var currentData = current.CloseApproachData;
        var otherData = other.CloseApproachData;
        
        currentData.CloseApproachDate = otherData.CloseApproachDate;
        currentData.EpochDateCloseApproach = otherData.EpochDateCloseApproach;
        currentData.RelativeVelocityKmh = otherData.RelativeVelocityKmh;
        currentData.MissDistanceKm = otherData.MissDistanceKm;
        currentData.OrbitingBody = otherData.OrbitingBody;
    }
}