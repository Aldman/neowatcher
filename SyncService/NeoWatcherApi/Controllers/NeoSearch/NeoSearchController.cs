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

    /// <summary>
    /// Поиск объектов по различным критериям.
    /// Поддерживает поиск по имени, фильтрацию по диаметру и статусу опасности.
    /// Результаты возвращаются с пагинацией.
    /// </summary>
    /// <param name="request">Параметры поиска включая имя, диапазон диаметров, статус опасности и пагинацию</param>
    /// <returns>Список найденных объектов или пустой результат</returns>
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
    
    /// <summary>
    /// Поиск объектов с похожими характеристиками.
    /// Алгоритм схожести на основе диаметра и скорости.
    /// Ранжирование по степени схожести.
    /// </summary>
    /// <param name="neoId">Идентификатор объекта</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Получение предложений для автодополнения поисковых запросов.
    /// Возвращает список уникальных имен объектов, начинающихся с указанного запроса.
    /// </summary>
    /// <param name="query">Поисковый запрос для автодополнения</param>
    /// <param name="limit">Максимальное количество предложений (по умолчанию 10)</param>
    /// <returns>Список предложений для автодополнения</returns>
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
}