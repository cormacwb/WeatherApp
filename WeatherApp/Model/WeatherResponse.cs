namespace WeatherApp.Model;

public class WeatherResponse
{
    public string? TimeZone { get; init; }
    public string? Location { get; init; }
    public DateTime LocalTime { get; init; }
    public decimal WindKph { get; init; }
    public decimal TemperatureC { get; init; }
    public decimal PrecipitationMm { get; init; }
    public decimal FeelsLikeC { get; init; }
    public DateTime Sunrise { get; init; }
    public DateTime Sunset { get; init; }
    public DateTime Moonrise { get; init; }
    public DateTime Moonset { get; init; }
    public string? MoonPhase { get; init; }
    public int MoonIllumination { get; init; }
}