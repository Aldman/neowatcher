using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoSearch;
using SyncService.EfComponents.DbSets;

namespace SyncService.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<DbNearEarthObject> ApplyFilters(
        this IQueryable<DbNearEarthObject> query,
        NeoSearchRequest request)
    {
        if (request.Name is not null)
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{request.Name}%"));
        
        if (request.MinDiameter is not null)
            query = query.Where(x => x.EstimatedDiameterMin >= request.MinDiameter);
        
        if (request.MaxDiameter is not null)
            query = query.Where(x => x.EstimatedDiameterMax <= request.MaxDiameter);
        
        if (request.IsHazardous is not null)
            query = query.Where(x => x.IsPotentiallyHazardous ==  request.IsHazardous);
        
        return query;
    }
}