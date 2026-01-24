namespace SyncService.DTOs.NeoReporting;

public class SummaryReportResponse
{
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int TotalCount { get; set; }
    public required int HazardousCount { get; set; }
    public required List<TopHazardousObject> TopHazardous { get; set; }
    public required List<TrendData> Trends { get; set; }
}