namespace SyncService.DTOs.NeoStatistics;

public class NeoAnomalyResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required double Diameter { get; set; }
    public required double Velocity { get; set; }
    public required double MissDistance { get; set; }
    public required List<string> AnomalyReasons { get; set; }
    public required double AnomalyScore { get; set; }
}