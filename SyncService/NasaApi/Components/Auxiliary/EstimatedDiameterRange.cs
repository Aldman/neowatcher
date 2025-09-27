using Newtonsoft.Json;

namespace SyncService.Api.Nasa.Components.Auxiliary;

[JsonObject]
public class EstimatedDiameterRange
{
    [JsonProperty("estimated_diameter_min")]
    public double EstimatedDiameterMin { get; set; }

    [JsonProperty("estimated_diameter_max")]
    public double EstimatedDiameterMax { get; set; }
}