namespace SyncService.DTOs.NeoReporting;

public class DailyReportResponse
{
    public required DateTime Date { get; set; }
    public required int TotalCount { get; set; }
    public required int HazardousCount { get; set; }
    public required double AvgDiameter { get; set; }
    public required double MaxDiameter { get; set; }
    public required List<HourlyStats> HourlyStats { get; set; }
}