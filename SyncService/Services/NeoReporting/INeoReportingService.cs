using SyncService.DTOs.NeoReporting;

namespace SyncService.Services.NeoReporting;

public interface INeoReportingService
{
    Task<DailyReportResponse?> GetDailyReportAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<WeeklyReportResponse?> GetWeeklyReportAsync(DateTime weekStart, CancellationToken cancellationToken = default);
    Task<SummaryReportResponse?> GetSummaryReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}