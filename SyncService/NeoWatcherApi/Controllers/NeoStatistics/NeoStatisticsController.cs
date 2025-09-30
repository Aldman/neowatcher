using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Services.NeoStatistics;

namespace SyncService.NeoWatcherApi.Controllers.NeoStatistics;

[ApiController]
[Route("neo/statistics")]
public class NeoStatisticsController : NeoControllerBase
{
    private readonly INeoStatisticsService _neoStatisticsService;

    public NeoStatisticsController(IMemoryCache memoryCache, INeoStatisticsService neoStatisticsService) : base(memoryCache)
    {
        _neoStatisticsService = neoStatisticsService;
    }
}