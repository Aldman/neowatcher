namespace SyncService.EfComponents.DbSets;

public class DbSetsDto
{
    public SyncDateTimes SyncDateTime { get; set; }
    public List<DbNearEarthObject> NearEarthObjects { get; set; }
    public List<DbCloseApproachData> CloseApproachData { get; set; }
}