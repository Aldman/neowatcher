using System.ComponentModel.DataAnnotations.Schema;

namespace SyncService.EfComponents.DbSets;

public class DbNearEarthObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Guid CloseApproachDataId { get; set; }
    public double EstimatedDiameterMin { get; set; }
    public double EstimatedDiameterMax { get; set; }
    public bool IsPotentiallyHazardous { get; set; }
    
    [NotMapped]
    public DbCloseApproachData CloseApproachData { get; set; }
}