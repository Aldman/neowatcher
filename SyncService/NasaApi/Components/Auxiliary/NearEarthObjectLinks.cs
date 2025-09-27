using Newtonsoft.Json;

namespace SyncService.NeoApiComponents;

public class NearEarthObjectLinks
{
    [JsonProperty("self")]
    public Uri Self { get; set; }
}