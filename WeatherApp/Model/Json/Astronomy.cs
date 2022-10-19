using Newtonsoft.Json;

namespace WeatherApp.Model.Json;

public record Astronomy
{
    public string? Sunrise { get; init; }

    public string? Sunset { get; init; }

    public string? Moonrise { get; init; }

    public string? Moonset { get; init; }

    [JsonProperty("moon_phase")]
    public string? MoonPhase { get; set; }

    [JsonProperty("moon_illumination")]
    public int MoonIllumination { get; set; }
}