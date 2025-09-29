namespace SyncService.DTOs.NeoSearch;

public class NeoSearchRequest
{
    public string? Name { get; set; }
    public double? MinDiameter { get; set; }
    public double? MaxDiameter { get; set; }
    public bool? IsHazardous { get; set; }
    public int? Limit { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}