using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.BusinessLogic;

namespace SyncService.NeoWatcherApi;

[ApiController]
[Route("neo")]
public class NeoApiController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly INeoStatService _neoStatService;

    private readonly MemoryCacheEntryOptions _cacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(10)
    };

    public NeoApiController(IMemoryCache memoryCache, INeoStatService neoStatService)
    {
        _memoryCache = memoryCache;
        _neoStatService = neoStatService;
    }

    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStats([FromQuery] NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        NeoFilterRequestValidator.Validate(filter);
        var filterKey = JsonConvert.SerializeObject(filter);
        if (_memoryCache.TryGetValue(filterKey, out var response))
            return Ok(response);
        
        var result = await _neoStatService.GetFilteredStatsAsync(filter, cancellationToken);
        
        _memoryCache.Set(filterKey, result, _cacheOptions);
        return Ok(result);
    }
}