using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.Helpers;
using SyncService.NeoWatcherApi.Controllers.NeoStats.NeoFilterRequestParts;
using SyncService.Services.NeoStats;

namespace SyncService.NeoWatcherApi.Controllers.NeoStats;

[ApiController]
[Route("neo")]
public class NeoApiController : NeoControllerBase
{
    private readonly INeoStatsService _neoStatsService;

    public NeoApiController(IMemoryCache memoryCache, INeoStatsService neoStatsService) : base(memoryCache)
    {
        _neoStatsService = neoStatsService;
    }

    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStats(
        [FromQuery] NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        NeoFilterRequestValidator.Validate(filter);
        var filterKey = CacheKeyGenerator.Generate(filter);
        if (MemoryCache.TryGetValue(filterKey, out var response))
            return Ok(response);
        
        var result = await _neoStatsService.GetFilteredStatsAsync(filter, cancellationToken);
        
        MemoryCache.Set(filterKey, result, CacheOptions);
        return Ok(result);
    }
}