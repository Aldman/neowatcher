using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.DTOs.NeoSearch;
using SyncService.Helpers;
using SyncService.Services.NeoSearch;

namespace SyncService.NeoWatcherApi.Controllers.NeoSearch;

[ApiController]
[Route("neo/search")]
public class NeoSearchController : NeoControllerBase
{
    private readonly INeoSearchService _searchService;

    public NeoSearchController(INeoSearchService searchService, IMemoryCache memoryCache) : base(memoryCache)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] NeoSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        NeoSearchRequestValidator.Validate(request);
        
        var cacheKey = CacheKeyGenerator.Generate(request);
        if (MemoryCache.TryGetValue(cacheKey, out var response))
            return Ok(response);

        var results = await _searchService.SearchAsync(request, cancellationToken);

        MemoryCache.Set(cacheKey, results, CacheOptions);
        return Ok(results);
    }
}