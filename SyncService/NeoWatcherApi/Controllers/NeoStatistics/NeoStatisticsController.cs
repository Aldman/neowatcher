using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Constants;
using SyncService.Helpers;
using SyncService.Services.NeoStatistics;

namespace SyncService.NeoWatcherApi.Controllers.NeoStatistics;

[ApiController]
[Route("neo/statistics")]
public class NeoStatisticsController : NeoControllerBase
{
    private readonly INeoStatisticsService _statisticsService;

    public NeoStatisticsController(IMemoryCache memoryCache, INeoStatisticsService statisticsService) : base(memoryCache)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet ("basic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBasicStatisticsAsync(
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        if (from > to)
            return ValidationProblem(
                detail: CommonExceptionTexts.FromMoreThanTo,
                statusCode: StatusCodes.Status416RangeNotSatisfiable
            );
        
        var cacheKey = CacheKeyGenerator.Generate(from, to);
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _statisticsService.GetBasicStatisticsAsync(from, to, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet ("DiameterDistribution")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetDiameterDistributionAsync(int buckets = 10, CancellationToken cancellationToken = default)
    {
        if (buckets <= 0)
            return ValidationProblem(
                detail: "The number of buckets must be greater than zero.",
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
            
        const string cacheKey = "DiameterDistribution";

        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _statisticsService.GetDiameterDistributionAsync(buckets, cancellationToken),
            returnNoContentIfNull: true);
    }
}