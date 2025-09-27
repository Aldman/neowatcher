namespace SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

public class NeoAnalyticsResponse
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public double MaxDiameter { get; set; }
    public double MinDiameter { get; set; }
    public double AvgVelocity { get; set; }
    public int HazardousCount { get; set; }
}