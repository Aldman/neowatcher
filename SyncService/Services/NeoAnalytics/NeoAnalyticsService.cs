using Microsoft.EntityFrameworkCore;
using SyncService.EfComponents.Repository;
using SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

namespace SyncService.Services.NeoAnalytics;

public class NeoAnalyticsService : INeoAnalyticsService
{
    private readonly INeoRepository _neoRepository;

    public NeoAnalyticsService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }
    
    public async Task<IEnumerable<NeoAnalyticsResponse>> GetDateRangeAnalyticsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        return await _neoRepository
            .GetNearEarthObjectsAsQueryable()
            .Include(x => x.CloseApproachData)
            .Where(x => x.CloseApproachData.CloseApproachDate >= from)
            .Where(x => x.CloseApproachData.CloseApproachDate <= to)
            .GroupBy(x => x.CloseApproachData.CloseApproachDate.Date)
            .Select(g => new NeoAnalyticsResponse
            {
                Date = g.Key,
                Count = g.Count(),
                MaxDiameter = g.Max(x => x.EstimatedDiameterMax),
                MinDiameter = g.Min(x => x.EstimatedDiameterMin),
                AvgVelocity = g.Average(x => x.CloseApproachData.RelativeVelocityKmh),
                HazardousCount = g.Count(x => x.IsPotentiallyHazardous)
            })
            .ToListAsync(cancellationToken);
    }
}