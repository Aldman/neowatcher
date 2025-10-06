namespace SyncService.DTOs.NeoReporting;

public class TrendData
{
    public required DateTime PeriodStart { get; set; }
    public required int TotalCount { get; set; }
    public required int HazardousCount { get; set; }
    public required double AvgDiameter { get; set; }
}