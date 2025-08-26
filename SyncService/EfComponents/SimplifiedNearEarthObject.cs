namespace SyncService.EfComponents;

public class SimplifiedNearEarthObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CloseApproachDate { get; set; }

    public double EstimatedDiameterMin { get; set; }
    public double EstimatedDiameterMax { get; set; }

    public bool IsPotentiallyHazardous { get; set; }

    public double RelativeVelocityKmh { get; set; }
    public double MissDistanceKm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}