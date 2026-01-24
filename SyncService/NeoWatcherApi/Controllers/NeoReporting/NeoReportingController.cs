using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Constants;
using SyncService.Helpers;
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
    
    /// <summary>
    /// Генерация ежедневного отчета.
    /// Создает отчет за конкретный день с группировкой по часам и статистиками по каждому часу.
    /// </summary>
    /// <param name="date">Дата для генерации отчета</param>
    /// <returns>Ежедневный отчет с почасовой статистикой</returns>
    [HttpGet ("DailyReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDailyReportAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"DailyReport:{date.ToString(CultureInfo.InvariantCulture)}";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetDailyReportAsync(date, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    /// <summary>
    /// Генерация еженедельного отчета.
    /// Создает отчет за неделю с группировкой по дням недели и сравнением с предыдущей неделей.
    /// </summary>
    /// <param name="week">Дата начала недели для генерации отчета</param>
    /// <returns>Еженедельный отчет с ежедневной статистикой и сравнением</returns>
    [HttpGet ("WeeklyReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetWeeklyReportAsync(DateTime week, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"WeeklyReport:{week.ToString(CultureInfo.InvariantCulture)}";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetWeeklyReportAsync(week, cancellationToken),
            returnNoContentIfNull: true);
    }
    
    /// <summary>
    /// Генерация сводного отчета за период.
    /// Создает комплексный отчет с топ-10 самых опасных объектов и анализом трендов по времени.
    /// </summary>
    /// <param name="from">Начальная дата периода</param>
    /// <param name="to">Конечная дата периода</param>
    /// <returns>Сводный отчет с топ-опасными объектами и трендами</returns>
    [HttpGet ("SummaryReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSummaryReportAsync(
        DateTime from, 
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (from > to)
            return ValidationProblem(
                detail: CommonExceptionTexts.FromMoreThanTo,
                statusCode: StatusCodes.Status416RangeNotSatisfiable
            );
        
        var cacheKey = $"SummaryReport:{CacheKeyGenerator.Generate(from, to)}))";
        
        return await GetFromCacheOrExecuteAsync(
            cacheKey: cacheKey,
            executeAsync: () => _reportingService.GetSummaryReportAsync(from, to, cancellationToken),
            returnNoContentIfNull: true);
    }
}