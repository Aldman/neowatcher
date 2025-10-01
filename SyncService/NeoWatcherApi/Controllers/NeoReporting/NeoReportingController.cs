using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
}