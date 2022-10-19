using Microsoft.AspNetCore.Mvc;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeather")]
    public async Task<WeatherResponse> Get(string cityName)
    {
        var weatherDetails = await _weatherService.GetWeatherResponseAsync(cityName);
        return weatherDetails;
    }
}