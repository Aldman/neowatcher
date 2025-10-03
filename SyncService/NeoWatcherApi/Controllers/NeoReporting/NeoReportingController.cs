using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Constants;
using SyncService.Helpers;
using SyncService.Services.NeoReporting;

namespace SyncService.NeoWatcherApi.Controllers.NeoReporting;

[ApiController]
[Route("neo/reporting")]
public class NeoReportingController : NeoControllerBase
{
    private readonly INeoReportingService _reportingService;

    public NeoReportingController(IMemoryCache memoryCache, INeoReportingService reportingService) : base(memoryCache)
    {
        _reportingService = reportingService;
    }
    
    [HttpGet ("DailyReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDailyReportAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"DailyReport:{date.ToString(CultureInfo.InvariantCulture)}";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetDailyReportAsync(date, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet ("WeeklyReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetWeeklyReportAsync(DateTime week, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"WeeklyReport:{week.ToString(CultureInfo.InvariantCulture)}";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetWeeklyReportAsync(week, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet ("SummaryReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSummaryReportAsync(
        DateTime from, 
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (from > to)
            return ValidationProblem(
                detail: CommonExceptionTexts.FromMoreThanTo,
                statusCode: StatusCodes.Status416RangeNotSatisfiable
            );
        
        var cacheKey = $"SummaryReport:{CacheKeyGenerator.Generate(from, to)}))";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetSummaryReportAsync(from, to, cancellationToken),
            returnNoContentIfNull: true);
    }
}