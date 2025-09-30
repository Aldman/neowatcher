namespace SyncService.DTOs.NeoStatistics;

public class NeoDistributionResponse
{
    public required double MinValue { get; set; }
    public required double MaxValue { get; set; }
    public required int Count { get; set; }
    public required double Percentage { get; set; }
}