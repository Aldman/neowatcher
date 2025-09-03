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
        current.CloseApproachDataId = other.CloseApproachDataId;
        current.CloseApproachData = other.CloseApproachData;
    }
}