using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoAnalytics;
using SyncService.EfComponents.Repository;
using SyncService.Extensions;
using SyncService.Helpers;

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

    public async Task<IEnumerable<NeoHazardousAnalysisResponse>> GetHazardousAnalysisAsync(
        CancellationToken cancellationToken = default)
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
                AvgDiameter = g.Average(x =>
                    CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin)),
                MaxDiameter = g.Max(x => x.EstimatedDiameterMax),
                AvgVelocity = g.Average(x => x.CloseApproachData.RelativeVelocityKmh),
                MinMissDistance = g.Min(x => x.CloseApproachData.MissDistanceKm)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<NeoComparisonResponse> ComparePeriodsAsync(
        DateTime period1Start,
        DateTime period1End,
        DateTime period2Start,
        DateTime period2End,
        CancellationToken cancellationToken = default)
    {
        var averageStats1 = await GetNeoPartForComparison(period1Start, period1End, cancellationToken);
        var averageStats2 = await GetNeoPartForComparison(period2Start, period2End, cancellationToken);

        return new NeoComparisonResponse
        {
            Count = GetPercentageChange(averageStats2.Count, averageStats1.Count),
            HazardousCount = GetPercentageChange(averageStats2.HazardousCount, averageStats1.HazardousCount),
            AvgDiameter = GetPercentageChange(averageStats2.AvgDiameter, averageStats1.AvgDiameter),
            MaxDiameter = GetPercentageChange(averageStats2.MaxDiameter, averageStats1.MaxDiameter),
            MinDiameter = GetPercentageChange(averageStats2.MinDiameter, averageStats1.MinDiameter),
            AvgVelocity = GetPercentageChange(averageStats2.AvgVelocity, averageStats1.AvgVelocity),
            MinMissDistance = GetPercentageChange(averageStats2.MinMissDistance, averageStats1.MinMissDistance),
        };
    }

    private async Task<NeoPartForComparison> GetNeoPartForComparison(
        DateTime periodStart,
        DateTime periodEnd,
        CancellationToken cancellationToken = default)
    {
        var range = await _neoRepository
            .GetNearEarthObjectsAsQueryable()
            .Include(x => x.CloseApproachData)
            .Where(x => x.CloseApproachData.CloseApproachDate >= periodStart)
            .Where(x => x.CloseApproachData.CloseApproachDate <= periodEnd)
            .ToListAsync(cancellationToken);

        return new NeoPartForComparison
        {
            Count = range.Count,
            HazardousCount = range.Count(x => x.IsPotentiallyHazardous),
            AvgDiameter = range.Average(x =>
                CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin)),
            MaxDiameter = range.Max(x => x.EstimatedDiameterMax),
            MinDiameter = range.Min(x => x.EstimatedDiameterMin),
            AvgVelocity = range.Average(x => x.CloseApproachData.RelativeVelocityKmh),
            MinMissDistance = range.Min(x => x.CloseApproachData.MissDistanceKm)
        };
    }
    
    private static string GetPercentageChange(double newValue, double oldValue) => 
        CalculationHelper
            .CalculateChangeRatio(newValue, oldValue)
            .ToPercentage();
}