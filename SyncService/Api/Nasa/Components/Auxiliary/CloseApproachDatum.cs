using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SyncService.NeoApiComponents;

public class CloseApproachDatum
{
    [JsonProperty("close_approach_date")]
    [JsonConverter(typeof(IsoDateTimeConverter))]
    public DateTime CloseApproachDate { get; set; }

    [JsonProperty("close_approach_date_full")]
    public DateTime CloseApproachDateFull { get; set; }

    [JsonProperty("epoch_date_close_approach")]
    public long EpochDateCloseApproach { get; set; }

    [JsonProperty("relative_velocity")]
    public RelativeVelocity RelativeVelocity { get; set; }

    [JsonProperty("miss_distance")]
    public MissDistance MissDistance { get; set; }

    [JsonProperty("orbiting_body")]
    public string OrbitingBody { get; set; }
}



