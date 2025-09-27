namespace SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

public class NeoPartForComparison
{
    public required double Count { get; set; }
    public required double HazardousCount { get; set; }
    public required double AvgDiameter { get; set; }
    public required double MaxDiameter { get; set; }
    public required double MinDiameter { get; set; }
    public required double AvgVelocity { get; set; }
    public required double MinMissDistance { get; set; }
}