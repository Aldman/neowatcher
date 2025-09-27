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
}