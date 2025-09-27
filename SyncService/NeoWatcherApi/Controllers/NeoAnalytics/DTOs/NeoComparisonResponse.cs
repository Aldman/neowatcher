namespace SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

public class NeoComparisonResponse
{
    public required string Count { get; set; }
    public required string HazardousCount { get; set; }
    public required string AvgDiameter { get; set; }
    public required string MaxDiameter { get; set; }
    public required string MinDiameter { get; set; }
    public required string AvgVelocity { get; set; }
    public required string MinMissDistance { get; set; }
}