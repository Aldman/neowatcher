namespace SyncService.DTOs.NeoStatistics;

public class NeoBasicStatsResponse
{
    public required int TotalCount { get; set; }
    public required double AverageDiameter { get; set; }
    public required double MedianDiameter { get; set; }
    public required double StandardDeviationDiameter { get; set; }
    public required double AverageVelocity { get; set; }
    public required double MedianVelocity { get; set; }
    public required double StandardDeviationVelocity { get; set; }
    public required int HazardousCount { get; set; }
    public required double HazardousPercentage { get; set; }
}