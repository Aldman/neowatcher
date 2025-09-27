using Newtonsoft.Json;

namespace SyncService.NeoApiComponents.Main;

[JsonObject]
public class NeoResponse
{
    [JsonProperty("links")]
    public NeoResponseLinks? Links { get; set; }

    [JsonProperty("element_count")]
    public long ElementCount { get; set; }

    [JsonProperty("near_earth_objects")]
    public Dictionary<DateTime, List<NearEarthObject>> NearEarthObjects { get; set; }
}