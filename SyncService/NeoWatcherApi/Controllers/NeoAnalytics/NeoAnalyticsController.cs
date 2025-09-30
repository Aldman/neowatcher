using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Constants;
using SyncService.Helpers;
using SyncService.Services.NeoAnalytics;

namespace SyncService.NeoWatcherApi.Controllers.NeoAnalytics;

[ApiController]
[Route("neo/analytics")]
public class NeoAnalyticsController : NeoControllerBase
{
    private readonly INeoAnalyticsService _analyticsService;

    public NeoAnalyticsController(IMemoryCache memoryCache, INeoAnalyticsService analyticsService) : base(memoryCache)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("byDateRange")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
    public async Task<IActionResult> GetAnalyticsByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (IsRangeInvalid(from, to, out var validationProblem)) 
            return validationProblem!;
        
        var cacheKey = CacheKeyGenerator.Generate(from, to);
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService.GetDateRangeAnalyticsAsync(from, to, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    private bool IsRangeInvalid(DateTime from, DateTime to, out IActionResult? validationProblem)
    {
        if (from >= to)
        {
            validationProblem = ValidationProblem(
                detail: CommonExceptionTexts.FromMoreThanTo,
                statusCode: StatusCodes.Status416RangeNotSatisfiable
            );
            return true;
        }

        validationProblem = null;
        return false;
    }
    
    [HttpGet("hazardousAnalysis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetHazardousAnalysisAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyGenerator.Generate();
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService.GetHazardousAnalysisAsync(cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet("comparePeriods")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
    public async Task<IActionResult> ComparePeriodsAsync(
        DateTime period1Start,
        DateTime period1End, 
        DateTime period2Start, 
        DateTime period2End, 
        CancellationToken cancellationToken = default)
    {
        if (IsRangeInvalid(period1Start, period1End, out var validationProblem1)) 
            return validationProblem1!;
        if (IsRangeInvalid(period2Start, period2End, out var validationProblem2)) 
            return validationProblem2!;
        
        var cacheKey = CacheKeyGenerator.Generate(period1Start, period1End, period2Start, period2End);
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService
                .ComparePeriodsAsync(period1Start, period1End, period2Start, period2End, cancellationToken),
            returnNoContentIfNull: false);
    }
}