namespace SyncService.DTOs.NeoReporting;

public class WeeklyReportResponse
{
    public required DateTime DateStart { get; set; }
    public required DateTime DateEnd { get; set; }
    public required int TotalCount { get; set; }
    public required int HazardousCount { get; set; }
    public required List<DailyStats> DailyStats { get; set; }
    public required WeeklyComparison Comparison { get; set; }
}