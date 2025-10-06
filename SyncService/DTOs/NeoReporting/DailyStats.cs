namespace SyncService.DTOs.NeoReporting;

public class DailyStats
{
    public required DateTime Date { get; set; }
    public required int TotalCount { get; set; }
    public required int HazardousCount { get; set; }
    public required double AverageDiameter { get; set; }
    public required double MaxDiameter { get; set; }
}