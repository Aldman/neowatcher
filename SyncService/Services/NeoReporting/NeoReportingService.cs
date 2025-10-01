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
}