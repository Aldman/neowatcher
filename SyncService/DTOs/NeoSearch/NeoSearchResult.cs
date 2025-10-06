namespace SyncService.DTOs.NeoSearch;

public class NeoSearchResult
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required double Diameter { get; set; }
    public required bool IsHazardous { get; set; }
    public required double Velocity { get; set; }
    public required double MissDistance { get; set; }
    public double? SimilarityScore { get; set; }
}