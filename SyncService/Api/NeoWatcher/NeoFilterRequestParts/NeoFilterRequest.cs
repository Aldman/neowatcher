namespace SyncService.Api.NeoWatcher.NeoFilterRequestParts;

public class NeoFilterRequest
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? IsHazardous { get; set; }
    public double? MinDiameter { get; set; }
    public double? MaxDiameter { get; set; }
    public SortBy? SortBy { get; set; }
    public SortDirections? SortDirection { get; set; }
}