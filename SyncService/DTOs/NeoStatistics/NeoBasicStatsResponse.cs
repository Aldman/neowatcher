namespace SyncService.DTOs.NeoStatistics;

public class NeoBasicStatsResponse
{
    public required int TotalCount { get; set; }
    public required double AvgDiameter { get; set; }
    public required double MedianDiameter { get; set; }
    public required double StdDevDiameter { get; set; }
    public required double AvgVelocity { get; set; }
    public required double MedianVelocity { get; set; }
    public required double StdDevVelocity { get; set; }
    public required int HazardousCount { get; set; }
    public required double HazardousPercentage { get; set; }
}