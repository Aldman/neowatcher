using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.EfComponents.Repository;

namespace SyncService.Api.NeoWatcher;

[ApiController]
[Route("neo")]
public class NeoApiController : ControllerBase
{
    private readonly INeoRepository _neoRepository;

    public NeoApiController(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromQuery] NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var query = _neoRepository.GetFilteredQuery(filter);

        var grouped = await query
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

        if (!filter.SortBy.HasValue)
            return Ok(grouped);

        var sorted = filter.SortBy switch
        {
            SortBy.Mass => filter.SortDir == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.MaxDiameter)
                : grouped.OrderBy(x => x.MaxDiameter),
            SortBy.Count => filter.SortDir == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.ObjectCount)
                : grouped.OrderBy(x => x.ObjectCount),
            _ => filter.SortDir == SortDirections.Desc
                ? grouped.OrderByDescending(x => x.Date)
                : grouped.OrderBy(x => x.Date),
        };

        return Ok(sorted);
    }
}