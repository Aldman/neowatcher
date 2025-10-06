using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoStatistics;
using SyncService.EfComponents.Repository;
using SyncService.Helpers;

namespace SyncService.Services.NeoStatistics;

public class NeoStatisticsService : INeoStatisticsService
{
    private readonly INeoRepository _neoRepository;

    public NeoStatisticsService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }
    
    public async Task<NeoBasicStatsResponse> GetBasicStatisticsAsync(
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = _neoRepository.GetFullNearEarthObjectsAsQueryable();

        if (from.HasValue)
            query = query.Where(x => x.CloseApproachData.CloseApproachDate >= from.Value);
        if (to.HasValue)
            query = query.Where(x => x.CloseApproachData.CloseApproachDate <= to.Value);

        var fromDb = await query
            .Select(x => new
            {
                Diameter = CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin),
                Velocity = x.CloseApproachData.RelativeVelocityKmh,
                IsHazardous = x.IsPotentiallyHazardous
            })
            .ToListAsync(cancellationToken);

        if (fromDb.Count == 0)
            return null;

        var diameters = fromDb.Select(x => x.Diameter);
        
        var velocities = fromDb.Select(x => x.Velocity);
        var hazardousCount = fromDb.Count(x => x.IsHazardous);
        
        return new NeoBasicStatsResponse
        {
            TotalCount = fromDb.Count,
            AverageDiameter = diameters.Average(),
            MedianDiameter = CalculationHelper.GetMedian(diameters),
            StandardDeviationDiameter = CalculationHelper.GetStandardDeviation(diameters),
            AverageVelocity = velocities.Average(),
            MedianVelocity = CalculationHelper.GetMedian(velocities),
            StandardDeviationVelocity = CalculationHelper.GetStandardDeviation(velocities),
            HazardousCount = hazardousCount,
            HazardousPercentage = (double)hazardousCount / fromDb.Count
        };
    }

    public async Task<IEnumerable<NeoDistributionResponse>?> GetDiameterDistributionAsync(
        int buckets = 10,
        CancellationToken cancellationToken = default)
    {
        var diameters = await _neoRepository
            .GetNearEarthObjectsAsQueryable()
            .Select(x => CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin))
            .ToListAsync(cancellationToken);

        if (diameters.Count == 0)
            return null;

        var min = diameters.Min();
        var max = diameters.Max();

        if (min.Equals(max))
            return
            [
                new NeoDistributionResponse
                {
                    MinValue = min,
                    MaxValue = max,
                    Count = diameters.Count,
                    Percentage = 1.0
                }
            ];
        
        var bucketSize = (max - min) / buckets;
        var total = diameters.Count;

        var distribution = Enumerable.Range(0, buckets)
            .Select(i => new NeoDistributionResponse
            {
                MinValue = min + i * bucketSize,
                MaxValue = i == buckets - 1
                    ? max 
                    : min + (i + 1) * bucketSize,
                Count = 0,
                Percentage = 0
            })
            .ToList();

        foreach (var d in diameters)
        {
            var index = (int)((d - min) / bucketSize);
            if (index == buckets) index = buckets - 1;
            distribution[index].Count++;
        }

        foreach (var bucket in distribution)
        {
            bucket.Percentage = (double)bucket.Count / total;
        }

        return distribution;
    }

        public async Task<IEnumerable<NeoAnomalyResponse>?> DetectAnomaliesAsync(CancellationToken cancellationToken = default)
    {
        var data = await _neoRepository
            .GetFullNearEarthObjectsAsQueryable()
            .Select(x => new
            {
                x.Id,
                x.Name,
                Diameter = CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin),
                Velocity = x.CloseApproachData.RelativeVelocityKmh,
                MissDistance = x.CloseApproachData.MissDistanceKm
            })
            .ToListAsync(cancellationToken);

        if (data.Count == 0)
            return null;

        var diameters = data.Select(x => x.Diameter).ToList();
        var velocities = data.Select(x => x.Velocity).ToList();
        var missDistances = data.Select(x => x.MissDistance).ToList();

        var meanDiameter = diameters.Average();
        var meanVelocity = velocities.Average();
        var meanMissDistance = missDistances.Average();

        var stdDiameter = CalculationHelper.GetStandardDeviation(diameters);
        var stdVelocity = CalculationHelper.GetStandardDeviation(velocities);
        var stdMissDistance = CalculationHelper.GetStandardDeviation(missDistances);

        const double threshold = 3.0;

        var anomalies = data
            .Select(x =>
            {
                var zDiameter = stdDiameter == 0 ? 0 : (x.Diameter - meanDiameter) / stdDiameter;
                var zVelocity = stdVelocity == 0 ? 0 : (x.Velocity - meanVelocity) / stdVelocity;
                var zMiss = stdMissDistance == 0 ? 0 : (x.MissDistance - meanMissDistance) / stdMissDistance;

                var reasons = new List<string>();
                if (Math.Abs(zDiameter) >= threshold)
                    reasons.Add($"Diameter {(zDiameter > 0 ? "high" : "low")} (z={zDiameter:F2})");
                if (Math.Abs(zVelocity) >= threshold)
                    reasons.Add($"Velocity {(zVelocity > 0 ? "high" : "low")} (z={zVelocity:F2})");
                if (Math.Abs(zMiss) >= threshold)
                    reasons.Add($"MissDistance {(zMiss > 0 ? "high" : "low")} (z={zMiss:F2})");

                var score = new[] { Math.Abs(zDiameter), Math.Abs(zVelocity), Math.Abs(zMiss) }.Max();

                return new
                {
                    x.Id,
                    x.Name,
                    x.Diameter,
                    x.Velocity,
                    x.MissDistance,
                    Reasons = reasons,
                    Score = score
                };
            })
            .Where(a => a.Reasons.Count > 0)
            .Select(a => new NeoAnomalyResponse
            {
                Id = a.Id,
                Name = a.Name,
                Diameter = a.Diameter,
                Velocity = a.Velocity,
                MissDistance = a.MissDistance,
                AnomalyReasons = a.Reasons,
                AnomalyScore = a.Score
            })
            .ToList();

        return anomalies.Count == 0 ? null : anomalies;
    }
}