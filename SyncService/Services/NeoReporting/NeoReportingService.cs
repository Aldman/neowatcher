using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoReporting;
using SyncService.EfComponents.Repository;
using SyncService.Helpers;

namespace SyncService.Services.NeoReporting;

public class NeoReportingService : INeoReportingService
{
    private readonly INeoRepository _neoRepository;

    public NeoReportingService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }

    public async Task<DailyReportResponse?> GetDailyReportAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var objects = await _neoRepository.GetFullNearEarthObjectsAsQueryable()
            .Where(x => x.CloseApproachData.CloseApproachDate.Date == date.Date)
            .Select(x => new
            {
                Date = x.CloseApproachData.CloseApproachDate,
                AverageDiameter = CalculationHelper.GetAverage(x.EstimatedDiameterMin, x.EstimatedDiameterMax),
                MaxDiameter = x.EstimatedDiameterMax,
                IsHazardous = x.IsPotentiallyHazardous
            })
            .ToListAsync(cancellationToken);
        
        if (objects.Count == 0) return null;

        var hourlyStats = objects
            .GroupBy(x => x.Date.Hour)
            .Select(x => new HourlyStats
            {
                Hour = x.Key,
                TotalCount = x.Count(),
                HazardousCount = x.Count(g => g.IsHazardous),
                AverageDiameter = x.Average(g => g.AverageDiameter),
                MaxDiameter = x.Max(g => g.MaxDiameter)
            })
            .OrderBy(x => x.Hour)
            .ToList();

        return new DailyReportResponse
        {
            Date = date,
            TotalCount = objects.Count,
            HazardousCount = objects.Count(g => g.IsHazardous),
            AverageDiameter = objects.Average(g => g.AverageDiameter),
            MaxDiameter = objects.Max(g => g.MaxDiameter),
            HourlyStats = hourlyStats
        };
    }

    public async Task<WeeklyReportResponse?> GetWeeklyReportAsync(DateTime weekStart, CancellationToken cancellationToken = default)
    {
        var previousWeekStart = weekStart.Subtract(TimeSpan.FromDays(7));
        var weekEnd = weekStart.Add(TimeSpan.FromDays(7));

        var weekStatsCalculation = (DateTime startDate) => _neoRepository.GetFullNearEarthObjectsAsQueryable()
            .Where(x => x.CloseApproachData.CloseApproachDate.Date >= startDate.Date)
            .Where(x => x.CloseApproachData.CloseApproachDate.Date <= startDate.AddDays(7))
            .Select(x => new
            {
                Date = x.CloseApproachData.CloseApproachDate,
                IsHazardous = x.IsPotentiallyHazardous,
                AverageDiameter = (x.EstimatedDiameterMin + x.EstimatedDiameterMax) / 2,
                MaxDiameter = x.EstimatedDiameterMax,
            });
        
        var currentWeekStats = weekStatsCalculation(weekStart);
        var previousWeekStats = weekStatsCalculation(previousWeekStart);

        var dailyStats = await currentWeekStats.GroupBy(x => x.Date.Date)
            .Select(g => new DailyStats
            {
                Date = g.Key,
                TotalCount = g.Count(),
                HazardousCount = g.Count(x => x.IsHazardous),
                AverageDiameter = g.Average(x => x.AverageDiameter),
                MaxDiameter = g.Max(x => x.MaxDiameter)
            }).ToListAsync(cancellationToken);

        var currentTotal = currentWeekStats.Count();
        var previousTotal = previousWeekStats.Count();
        var currentHazardous = currentWeekStats.Count(x => x.IsHazardous);
        var previousHazardous = previousWeekStats.Count(x => x.IsHazardous);
        var weeklyComparison = new WeeklyComparison
        {
            CurrentTotal = currentTotal,
            PreviousTotal = previousTotal,
            CurrentHazardous = currentHazardous,
            PreviousHazardous = previousHazardous,
            DeltaTotal = currentTotal - previousTotal,
            DeltaHazardous = currentHazardous - previousHazardous,
            PercentChangeTotal = CalculationHelper.CalculateChangeRatioWithInfinite(currentTotal, previousTotal, double.MaxValue),
            PercentChangeHazardous = CalculationHelper.CalculateChangeRatioWithInfinite(currentHazardous, previousHazardous, double.MaxValue),
        };

        return new WeeklyReportResponse
        {
            DateStart = weekStart,
            DateEnd = weekEnd,
            TotalCount = currentTotal,
            HazardousCount = currentHazardous,
            DailyStats = dailyStats,
            Comparison = weeklyComparison
        };
    }
}