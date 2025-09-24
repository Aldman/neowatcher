namespace SyncService.Api.NeoWatcher;

public class NeoStatResponse
{
    public DateTime Date { get; set; }
    public int ObjectCount { get; set; }
    public double MaxDiameter { get; set; }
    public double AvgVelocity { get; set; }
    public bool HasHazardousObjects { get; set; }
}