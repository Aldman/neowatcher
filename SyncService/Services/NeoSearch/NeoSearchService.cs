using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoSearch;
using SyncService.EfComponents.Repository;
using SyncService.Extensions;
using SyncService.Helpers;
using SyncService.Helpers.CompareHelpers;

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

        var normalizer = new DoubleComparingNormalizer(minDiameter, maxDiameter, minVelocity, maxVelocity);
        var normalisedForComparison = normalizer.NormalizeForComparing(forComparison); 
        
        return await query.Where(x => x.Id != neoId)
            .Select(x => new
            {
                OriginalObj = x,
                NormalizedForComparing = normalizer.NormalizeForComparing(x)
            })
            .Select(x => new NeoSearchResult
            {
                Id = x.OriginalObj.Id,
                Name = x.OriginalObj.Name,
                Diameter = CalculationHelper.GetAverage(x.OriginalObj.EstimatedDiameterMax, x.OriginalObj.EstimatedDiameterMin),
                IsHazardous = x.OriginalObj.IsPotentiallyHazardous,
                Velocity = x.OriginalObj.CloseApproachData.RelativeVelocityKmh,
                MissDistance = x.OriginalObj.CloseApproachData.MissDistanceKm,
                SimilarityScore = ComparingPropertiesComparer.CalculateSimilarity(normalisedForComparison, x.NormalizedForComparing)
            })
            .OrderByDescending(x => x.SimilarityScore)
            .Take(DefaultLimit)
            .ToListAsync(cancellationToken);
    }
}