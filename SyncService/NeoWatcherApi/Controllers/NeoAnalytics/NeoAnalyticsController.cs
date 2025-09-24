using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
}