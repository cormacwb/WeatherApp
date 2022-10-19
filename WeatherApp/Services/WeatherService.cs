using AutoMapper;
using Newtonsoft.Json.Linq;
using WeatherApp.Model;
using WeatherApp.Model.Json;

namespace WeatherApp.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IMapper mapper, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WeatherResponse> GetWeatherResponseAsync(string cityName)
    {
        using var getWeather = _httpClient.GetAsync($"current.json?q={cityName}");
        using var getAstronomy = _httpClient.GetAsync($"astronomy.json?q={cityName}");
        using var getTimeZone = _httpClient.GetAsync($"timezone.json?q={cityName}");

        try
        {
            await Task.WhenAll(getWeather, getAstronomy, getTimeZone);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "At least one call to Weather API failed. See below for specific details");
        }

        var weather = await ParseAsync(getWeather, body => body["current"]!.ToObject<Weather>());
        var astronomy = await ParseAsync(getAstronomy, body => body["astronomy"]!["astro"]!.ToObject<Astronomy>());
        var timezone = await ParseAsync(getTimeZone, body => body["location"]!.ToObject<Timezone>());

        return _mapper.Map<WeatherResponse>((weather, astronomy, timezone));
    }

    private async Task<T> ParseAsync<T>(Task<HttpResponseMessage> response, Func<JObject, T> getObject) where T : new()
    {
        if (HttpCallFailed(response)) return new T();

        var body = await response.Result.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(body);

        return getObject(parsed);
    }

    private bool HttpCallFailed(Task<HttpResponseMessage> response)
    {
        const string errorMessage = "Call to Weather API failed";
        if (response.IsFaulted)
        {
            _logger.Log(LogLevel.Error, response.Exception, errorMessage);
            return true;
        }

        try
        {
            response.Result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, errorMessage);
            return true;
        }

        return false;
    }
}
