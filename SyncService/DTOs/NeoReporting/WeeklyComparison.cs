namespace SyncService.DTOs.NeoReporting;

public class WeeklyComparison
{
    public required int CurrentTotal { get; set; }
    public required int PreviousTotal { get; set; }
    public required int CurrentHazardous { get; set; }
    public required int PreviousHazardous { get; set; }
    public required int DeltaTotal { get; set; }
    public required int DeltaHazardous { get; set; }
    public required double PercentChangeTotal { get; set; }
    public required double PercentChangeHazardous { get; set; }
}