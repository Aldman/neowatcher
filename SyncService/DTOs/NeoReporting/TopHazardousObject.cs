namespace SyncService.DTOs.NeoReporting;

public class TopHazardousObject
{
    public required string Id { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public required double Diameter { get; set; }
    public required double Velocity { get; set; }
    public required double MissDistance { get; set; }
    public required DateTime CloseApproachDate { get; set; }
    public required double DangerScore { get; set; }
}