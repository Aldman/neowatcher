namespace SyncService.NeoWatcherApi.Controllers.NeoAnalytics.DTOs;

public class NeoHazardousAnalysisResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public bool IsHazardous { get; set; }
    public int Count { get; set; }
    public double AvgDiameter { get; set; }
    public double MaxDiameter { get; set; }
    public double AvgVelocity { get; set; }
    public double MinMissDistance { get; set; }
}