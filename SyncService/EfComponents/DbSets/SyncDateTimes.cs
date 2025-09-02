namespace SyncService.EfComponents.DbSets;

public class SyncDateTimes
{
    public Guid Id { get; set; }
    public DateTime SyncTime { get; set; }
    public List <DbNearEarthObject> NearEarthObjects { get; set; }
}