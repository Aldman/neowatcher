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
    [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
    public async Task<IActionResult> GetAnalyticsByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (IsRangeInvalid(from, to, out var validationProblem)) 
            return validationProblem!;
        
        var cacheKey = CacheKeyGenerator.Generate(from, to);
        if (MemoryCache.TryGetValue(cacheKey, out var response))
            return Ok(response);

        var results = await _analyticsService
            .GetDateRangeAnalyticsAsync(from, to, cancellationToken);
        
        MemoryCache.Set(cacheKey, results, CacheOptions);
        return Ok(results);
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
}