using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.EfComponents.DbSets;
using SyncService.EfComponents.Repository;

namespace SyncService.Api.NeoWatcher;

[ApiController]
[Route("neo")]
public class NeoApiController : ControllerBase
{
    private readonly INeoRepository _neoRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(10)
    };

    public NeoApiController(INeoRepository neoRepository, IMemoryCache memoryCache)
    {
        _neoRepository = neoRepository;
        _memoryCache = memoryCache;
    }

    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStats([FromQuery] NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        NeoFilterRequestValidator.Validate(filter);
        var filterKey = JsonConvert.SerializeObject(filter);
        if (_memoryCache.TryGetValue(filterKey, out var response))
            return Ok(response);
        
        var query = _neoRepository.GetFilteredQuery(filter);
        var grouped = await GroupByDateAsync(query, cancellationToken);

        IEnumerable<NeoStatResponse> toReturn = filter.SortBy.HasValue
            ? Sort(filter, grouped)
            : grouped;
        
        _memoryCache.Set(filterKey, toReturn, _cacheOptions);
        return Ok(toReturn);
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