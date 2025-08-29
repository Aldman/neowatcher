namespace SyncService.EfComponents.DbSets;

public class DbCloseApproachData
{
    public Guid Id { get; set; }
    public DateTime CloseApproachDate { get; set; }
    public long EpochDateCloseApproach { get; set; }
    public double RelativeVelocityKmh { get; set; }
    public double MissDistanceKm { get; set; }
    public string? OrbitingBody { get; set; }
}