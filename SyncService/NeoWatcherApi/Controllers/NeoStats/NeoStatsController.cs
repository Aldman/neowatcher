using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.Helpers;
using SyncService.NeoWatcherApi.Controllers.NeoStats.NeoFilterRequestParts;
using SyncService.Services.NeoStats;

namespace SyncService.NeoWatcherApi.Controllers.NeoStats;

[ApiController]
[Route("neo")]
public class NeoStatsController : NeoControllerBase
{
    private readonly INeoStatsService _neoStatsService;

    public NeoStatsController(IMemoryCache memoryCache, INeoStatsService neoStatsService) : base(memoryCache)
    {
        _neoStatsService = neoStatsService;
    }

    /// <summary>
    /// Получение статистических данных с фильтрацией.
    /// Возвращает отфильтрованные статистики по объектам с возможностью сортировки.
    /// </summary>
    /// <param name="filter">Параметры фильтрации и сортировки</param>
    /// <returns>Отфильтрованные статистические данные</returns>
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStats(
        [FromQuery] NeoFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        NeoFilterRequestValidator.Validate(filter);
        var cacheKey = CacheKeyGenerator.Generate(filter);
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _neoStatsService.GetFilteredStatsAsync(filter, cancellationToken),
            returnNoContentIfNull: true);
    }
}