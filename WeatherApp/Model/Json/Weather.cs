using Newtonsoft.Json;

namespace WeatherApp.Model.Json;

public record Weather
{
    [JsonProperty("wind_kph")]
    public decimal WindKph { get; init; }
    
    [JsonProperty("temp_c")]
    public decimal TemperatureC { get; init; }
    
    [JsonProperty("precip_mm")]
    public decimal PrecipitationMm { get; init; }
    
    [JsonProperty("feelslike_c")]
    public decimal FeelsLikeC { get; init; }
}