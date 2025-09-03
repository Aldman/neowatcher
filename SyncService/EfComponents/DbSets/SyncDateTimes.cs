namespace SyncService.EfComponents.DbSets;

public class SyncDateTimes
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime SyncTime { get; set; } =  DateTime.Now;
    
    public List <DbNearEarthObject> NearEarthObjects { get; set; }  = new();
}