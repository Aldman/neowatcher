using Microsoft.EntityFrameworkCore;
using SyncService.Api.NeoWatcher;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.EfComponents.DbSets;
using SyncService.EfComponents.Repository;

namespace SyncService.BusinessLogic;

public class NeoStatService : INeoStatService
{
    private readonly INeoRepository _neoRepository;

    public NeoStatService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }
    
    public async Task<IEnumerable<NeoStatResponse>> GetFilteredStatsAsync(
        NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var query = _neoRepository.GetFilteredQuery(filter);
        var grouped = await GroupByDateAsync(query, cancellationToken);

        return filter.SortBy.HasValue
            ? Sort(filter, grouped)
            : grouped;
    }
    
    private static async Task<List<NeoStatResponse>> GroupByDateAsync(
        IQueryable<DbNearEarthObject> query,
        CancellationToken cancellationToken = default) =>
        
        await query
            .Include(x => x.CloseApproachData)
            .GroupBy(x => x.CloseApproachData.CloseApproachDate.Date)
            .Select(g => new NeoStatResponse
            {
                Date = g.Key,
                ObjectCount = g.Count(),
                MaxDiameter = g.Max(x => x.EstimatedDiameterMax),
                AvgVelocity = g.Average(x => x.CloseApproachData.RelativeVelocityKmh),
                HasHazardousObjects = g.Any(x => x.IsPotentiallyHazardous)
            })
            .ToListAsync(cancellationToken);
    
    private static IOrderedEnumerable<NeoStatResponse> Sort(NeoFilterRequest filter, List<NeoStatResponse> grouped) =>
        filter.SortBy switch
        {
            SortBy.Mass => filter.SortDirection == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.MaxDiameter)
                : grouped.OrderBy(x => x.MaxDiameter),
            SortBy.Count => filter.SortDirection == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.ObjectCount)
                : grouped.OrderBy(x => x.ObjectCount),
            _ => filter.SortDirection == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.Date)
                : grouped.OrderBy(x => x.Date)
        };
}