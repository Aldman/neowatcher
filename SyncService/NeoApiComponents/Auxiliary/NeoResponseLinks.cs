using Newtonsoft.Json;

namespace SyncService.NeoApiComponents;

public class NeoResponseLinks
{
    [JsonProperty("next")]
    public Uri Next { get; set; }

    [JsonProperty("previous")]
    public Uri Previous { get; set; }

    [JsonProperty("self")]
    public Uri Self { get; set; }
}