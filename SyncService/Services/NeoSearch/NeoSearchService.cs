using Microsoft.EntityFrameworkCore;
using SyncService.DTOs.NeoSearch;
using SyncService.EfComponents.Repository;
using SyncService.Extensions;
using SyncService.Helpers;

namespace SyncService.Services.NeoSearch;

public class NeoSearchService : INeoSearchService
{
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
}