using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.BusinessLogic;

namespace SyncService.NeoWatcherApi.Controllers.NeoStats;

[ApiController]
[Route("neo")]
public class NeoApiController : NeoControllerBase
{
    private readonly INeoStatService _neoStatService;

    public NeoApiController(IMemoryCache memoryCache, INeoStatService neoStatService) : base(memoryCache)
    {
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
        if (MemoryCache.TryGetValue(filterKey, out var response))
            return Ok(response);
        
        var result = await _neoStatService.GetFilteredStatsAsync(filter, cancellationToken);
        
        MemoryCache.Set(filterKey, result, CacheOptions);
        return Ok(result);
    }
}