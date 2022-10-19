using Newtonsoft.Json;

namespace WeatherApp.Model.Json;

public record Timezone
{
    public string? Name { get; init; }

    [JsonProperty("tz_id")]
    public string? TimezoneId { get; init; }

    public string? LocalTime { get; init; }
}
