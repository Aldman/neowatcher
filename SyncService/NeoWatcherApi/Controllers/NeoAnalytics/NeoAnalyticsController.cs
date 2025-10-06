using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Constants;
using SyncService.Helpers;
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

    /// <summary>
    /// Получение аналитических данных по диапазону дат.
    /// Группирует объекты по датам и вычисляет статистики: общее количество, количество опасных объектов,
    /// максимальный/минимальный диаметр, средняя скорость.
    /// </summary>
    /// <param name="from">Начальная дата диапазона</param>
    /// <param name="to">Конечная дата диапазона</param>
    /// <returns>Аналитические данные по датам в указанном диапазоне</returns>
    [HttpGet("byDateRange")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
    public async Task<IActionResult> GetAnalyticsByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (IsRangeInvalid(from, to, out var validationProblem)) 
            return validationProblem!;
        
        var cacheKey = CacheKeyGenerator.Generate(from, to);
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService.GetDateRangeAnalyticsAsync(from, to, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    private bool IsRangeInvalid(DateTime from, DateTime to, out IActionResult? validationProblem)
    {
        if (from >= to)
        {
            validationProblem = ValidationProblem(
                detail: CommonExceptionTexts.FromMoreThanTo,
                statusCode: StatusCodes.Status416RangeNotSatisfiable
            );
            return true;
        }

        validationProblem = null;
        return false;
    }
    
    /// <summary>
    /// Анализ опасных объектов по времени.
    /// Группирует объекты по году, месяцу и статусу опасности для анализа распределения опасных объектов.
    /// </summary>
    /// <returns>Анализ распределения опасных объектов по времени</returns>
    [HttpGet("hazardousAnalysis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetHazardousAnalysisAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyGenerator.Generate();
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService.GetHazardousAnalysisAsync(cancellationToken),
            returnNoContentIfNull: true);
    }
    
    /// <summary>
    /// Сравнение двух временных периодов.
    /// Вычисляет изменения в процентах между периодами и анализирует тренды.
    /// </summary>
    /// <param name="period1Start">Начало первого периода</param>
    /// <param name="period1End">Конец первого периода</param>
    /// <param name="period2Start">Начало второго периода</param>
    /// <param name="period2End">Конец второго периода</param>
    /// <returns>Результат сравнения двух периодов с анализом изменений</returns>
    [HttpGet("comparePeriods")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
    public async Task<IActionResult> ComparePeriodsAsync(
        DateTime period1Start,
        DateTime period1End, 
        DateTime period2Start, 
        DateTime period2End, 
        CancellationToken cancellationToken = default)
    {
        if (IsRangeInvalid(period1Start, period1End, out var validationProblem1)) 
            return validationProblem1!;
        if (IsRangeInvalid(period2Start, period2End, out var validationProblem2)) 
            return validationProblem2!;
        
        var cacheKey = CacheKeyGenerator.Generate(period1Start, period1End, period2Start, period2End);
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _analyticsService
                .ComparePeriodsAsync(period1Start, period1End, period2Start, period2End, cancellationToken),
            returnNoContentIfNull: false);
    }
}