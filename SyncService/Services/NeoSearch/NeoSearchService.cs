using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoSearch;
using SyncService.EfComponents.Repository;
using SyncService.Extensions;
using SyncService.Helpers;

namespace SyncService.Services.NeoSearch;

public class NeoSearchService : INeoSearchService
{
    private const int DefaultLimit = 50;
    private readonly INeoRepository _neoRepository;

    public NeoSearchService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }
    
    public async Task<IEnumerable<NeoSearchResult>> SearchAsync(
        NeoSearchRequest request, 
        CancellationToken cancellationToken = default)
    {
        var limit = GetLimit(request);
        
        return await _neoRepository.GetFullNearEarthObjectsAsQueryable()
            .ApplyFilters(request)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(limit)
            .Select(x => new NeoSearchResult
            {
                Id = x.Id,
                Name = x.Name,
                Diameter = CalculationHelper.GetAverage(x.EstimatedDiameterMax, x.EstimatedDiameterMin),
                IsHazardous = x.IsPotentiallyHazardous,
                Velocity = x.CloseApproachData.RelativeVelocityKmh,
                MissDistance = x.CloseApproachData.MissDistanceKm,
                SimilarityScore = 100
            })
            .ToListAsync(cancellationToken);
    }

    private static int GetLimit(NeoSearchRequest request)
    {
        return request.Limit.HasValue
            ? Math.Min(request.Limit.Value, request.PageSize)
            : request.PageSize;
    }

    public async Task<IEnumerable<NeoSearchResult>?> FindSimilarAsync(string neoId, CancellationToken cancellationToken)
    {
        var query = _neoRepository.GetFullNearEarthObjectsAsQueryable();
        
        var forComparison = await query.FirstOrDefaultAsync(x => x.Id == neoId,  cancellationToken);
        if (forComparison == null)
            return null;
        
        var minDiameter = await query.MinAsync(x => x.EstimatedDiameterMin, cancellationToken);
        var maxDiameter = await query.MaxAsync(x => x.EstimatedDiameterMax, cancellationToken);
        var maxVelocity = await query.MaxAsync(x => x.CloseApproachData.RelativeVelocityKmh, cancellationToken);
        var minVelocity = await query.MinAsync(x => x.CloseApproachData.RelativeVelocityKmh, cancellationToken);

        var diameterRange = new NumberPropertyRange<double>(minDiameter, maxDiameter);
        var velocityRange = new NumberPropertyRange<double>(minVelocity, maxVelocity);

        var normalizeValue = (double value, NumberPropertyRange<double> range)
            => range.DiffIsZero
                ? 0
                : (value - range.Min) / range.Diff;

        var forComparisonMinDiameter = normalizeValue(forComparison.EstimatedDiameterMin, diameterRange);
        var forComparisonMaxDiameter =  normalizeValue(forComparison.EstimatedDiameterMax, diameterRange);
        var forComparisonVelocity = normalizeValue(forComparison.CloseApproachData.RelativeVelocityKmh, velocityRange);
        
        return await query.Where(x => x.Id != neoId)
            .Select(x => new
            {
                OriginalObj = x,
                NormalizedMinDiameter = diameterRange.DiffIsZero
                    ? 0
                    : (x.EstimatedDiameterMin - diameterRange.Min) / diameterRange.Diff,
                NormalizedMaxDiameter = diameterRange.DiffIsZero
                    ? 0
                    : (x.EstimatedDiameterMax - diameterRange.Min) / diameterRange.Diff,
                NormalizedVelocity = velocityRange.DiffIsZero
                    ? 0
                    : (x.CloseApproachData.RelativeVelocityKmh - velocityRange.Min) / velocityRange.Diff
            })
            .Select(x => new NeoSearchResult
            {
                Id = x.OriginalObj.Id,
                Name = x.OriginalObj.Name,
                Diameter = CalculationHelper.GetAverage(x.OriginalObj.EstimatedDiameterMax, x.OriginalObj.EstimatedDiameterMin),
                IsHazardous = x.OriginalObj.IsPotentiallyHazardous,
                Velocity = x.OriginalObj.CloseApproachData.RelativeVelocityKmh,
                MissDistance = x.OriginalObj.CloseApproachData.MissDistanceKm,
                SimilarityScore = 1 / (1 + Math.Sqrt(
                    (forComparisonMinDiameter - x.NormalizedMinDiameter) * (forComparisonMinDiameter - x.NormalizedMinDiameter)
                    + (forComparisonMaxDiameter - x.NormalizedMaxDiameter) * (forComparisonMaxDiameter - x.NormalizedMaxDiameter)
                    + (forComparisonVelocity - x.NormalizedVelocity) * (forComparisonVelocity - x.NormalizedVelocity)
                    ))
            })
            .OrderByDescending(x => x.SimilarityScore)
            .Take(DefaultLimit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>?> GetSuggestionsAsync(
        string query,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        return await _neoRepository.GetNearEarthObjectsAsQueryable()
            .Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
            .Select(x => x.Name)
            .Distinct()
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}