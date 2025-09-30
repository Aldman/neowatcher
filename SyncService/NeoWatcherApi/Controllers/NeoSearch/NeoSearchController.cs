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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] NeoSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        NeoSearchRequestValidator.Validate(request);
        
        var cacheKey = CacheKeyGenerator.Generate(request);
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _searchService.SearchAsync(request, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet("FindSimilar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> FindSimilarAsync(string neoId, CancellationToken cancellationToken = default)
    {
        return await GetFromCacheOrExecuteAsync(
            cacheKey: neoId,
            executeAsync: () => _searchService.FindSimilarAsync(neoId, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    [HttpGet("GetSuggestions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSuggestionsAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        return await GetFromCacheOrExecuteAsync(
            cacheKey: query,
            executeAsync: () => _searchService.GetSuggestionsAsync(query, limit, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    private async Task<IActionResult> GetFromCacheOrExecuteAsync<T>(
        string cacheKey, 
        Func<Task<T>> executeAsync, 
        bool returnNoContentIfNull = false)
    {
        if (MemoryCache.TryGetValue(cacheKey, out var response))
            return Ok(response);

        var results = await executeAsync();

        if (returnNoContentIfNull && results is null)
            return NoContent();

        MemoryCache.Set(cacheKey, results, CacheOptions);
        return Ok(results);
    }
}