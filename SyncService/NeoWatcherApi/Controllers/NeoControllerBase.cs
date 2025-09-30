using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace SyncService.NeoWatcherApi.Controllers;

public class NeoControllerBase : ControllerBase
{
    protected readonly IMemoryCache MemoryCache;

    protected readonly MemoryCacheEntryOptions CacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(10)
    };

    public NeoControllerBase(IMemoryCache memoryCache)
    {
        MemoryCache = memoryCache;
    }
    
    protected async Task<IActionResult> GetFromCacheOrExecuteAsync<T>(
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