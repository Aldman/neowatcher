using System.ComponentModel.DataAnnotations.Schema;

namespace SyncService.EfComponents.DbSets;

public class SyncDateTimes
{
    public Guid Id { get; set; }
    public DateTime SyncTime { get; set; }
    public string NearEarthObjectId { get; set; }
    
    [NotMapped]
    public DbNearEarthObject NearEarthObject { get; set; }
}