using Newtonsoft.Json;

namespace SyncService.NeoApiComponents;

public class EstimatedDiameter
{
    [JsonProperty("kilometers")]
    public EstimatedDiameterRange Kilometers { get; set; }

    [JsonProperty("meters")]
    public EstimatedDiameterRange Meters { get; set; }

    [JsonProperty("miles")]
    public EstimatedDiameterRange Miles { get; set; }

    [JsonProperty("feet")]
    public EstimatedDiameterRange Feet { get; set; }
}