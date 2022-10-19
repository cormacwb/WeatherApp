using WeatherApp.Model;

namespace WeatherApp.Services;

public interface IWeatherService
{
    Task<WeatherResponse> GetWeatherResponseAsync(string cityName);
}