namespace SyncService.DTOs.NeoAnalytics;

public class NeoPartForComparison
{
    public required double Count { get; set; }
    public required double HazardousCount { get; set; }
    public required double AvgDiameter { get; set; }
    public required double MaxDiameter { get; set; }
    public required double MinDiameter { get; set; }
    public required double AvgVelocity { get; set; }
    public required double MinMissDistance { get; set; }
    
    public static NeoPartForComparison Empty => new()
    {
        Count = 0,
        HazardousCount = 0,
        AvgDiameter = 0,
        MaxDiameter = 0,
        MinDiameter = 0,
        AvgVelocity = 0,
        MinMissDistance = 0,
    };
}