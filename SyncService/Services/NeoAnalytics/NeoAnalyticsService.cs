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
    
    public async Task<IEnumerable<NeoHazardousAnalysisResponse>> GetHazardousAnalysisAsync(CancellationToken cancellationToken = default)
    {
        return await _neoRepository
            .GetNearEarthObjectsAsQueryable()
            .Include(x => x.CloseApproachData)
            .GroupBy(x => new
            {
                x.CloseApproachData.CloseApproachDate.Year,
                x.CloseApproachData.CloseApproachDate.Month,
                IsHazardous = x.IsPotentiallyHazardous,
            })
            .Select(g => new NeoHazardousAnalysisResponse
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                IsHazardous = g.Key.IsHazardous,
                Count = g.Count(),
                AvgDiameter = g.Average(x => (x.EstimatedDiameterMax + x.EstimatedDiameterMin) / 2),
                MaxDiameter = g.Max(x => x.EstimatedDiameterMax),
                AvgVelocity = g.Average(x => x.CloseApproachData.RelativeVelocityKmh),
                MinMissDistance = g.Min(x => x.CloseApproachData.MissDistanceKm)
            })
            .ToListAsync(cancellationToken);
    }
}