using AutoMapper;
using FluentAssertions;
using WeatherApp.Mapping;
using WeatherApp.Model;
using WeatherApp.Model.Json;

namespace WeatherApp.Tests.Mapping;

[TestFixture]
public class WeatherResponseProfileTests
{
    private IMapper _mapper = null!;
    private MapperConfiguration _configuration = null!;

    [SetUp]
    public void SetUp()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<WeatherResponseProfile>();
        });
        _mapper = new Mapper(_configuration);
    }

    [Test]
    public void WeatherResponseProfileIsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    public void ValidWeatherObject_MapsCorrectly()
    {
        var validWeather = CreateValidWeather();

        var result = _mapper.Map<WeatherResponse>((validWeather, CreateValidAstronomy(), CreateValidTimezone()));

        result.TemperatureC.Should().Be(validWeather.TemperatureC);
        result.WindKph.Should().Be(validWeather.WindKph);
        result.FeelsLikeC.Should().Be(validWeather.FeelsLikeC);
        result.PrecipitationMm.Should().Be(validWeather.PrecipitationMm);
    }

    private static Weather CreateValidWeather()
    {
        return new Weather
        {
            TemperatureC = 1,
            FeelsLikeC = 2,
            PrecipitationMm = 3,
            WindKph = 4,
        };
    }

    private static Astronomy CreateValidAstronomy()
    {
        return new Astronomy
        {
            Moonrise = "08:00 PM",
            Moonset = "02:00 PM",
            Sunrise = "06:01 AM",
            Sunset = "07:04 PM",
            MoonIllumination = 44,
            MoonPhase = "Third Quarter"
        };
    }

    private static Timezone CreateValidTimezone()
    {
        return new Timezone
        {
            Name = "Honolulu",
            LocalTime = "2022-10-18 8:14",
            TimezoneId = "Pacific/Honolulu"
        };
    }

    [Test]
    public void ValidAstronomyObject_MapsCorrectly()
    {
        var validAstronomy = CreateValidAstronomy();

        var result = _mapper.Map<WeatherResponse>((CreateValidWeather(), validAstronomy, CreateValidTimezone()));

        result.Moonrise.Should().Be(new DateTime(2022, 10, 18, 20, 0, 0, DateTimeKind.Local));
        result.Moonset.Should().Be(new DateTime(2022, 10, 18, 14, 0, 0, DateTimeKind.Local));
        result.Sunrise.Should().Be(new DateTime(2022, 10, 18, 6, 1, 0, DateTimeKind.Local));
        result.Sunset.Should().Be(new DateTime(2022, 10, 18, 19, 4, 0, DateTimeKind.Local));
        result.MoonIllumination.Should().Be(44);
        result.MoonPhase.Should().Be("Third Quarter");
    }

    [Test]
    public void ValidTimezoneObject_MapsCorrectly()
    {
        var validTimezone = CreateValidTimezone();

        var result = _mapper.Map<WeatherResponse>((CreateValidWeather(), CreateValidAstronomy(), validTimezone));

        result.Location.Should().Be(validTimezone.Name);
        result.TimeZone.Should().Be(validTimezone.TimezoneId);
        result.LocalTime.Should().Be(new DateTime(2022, 10, 18, 8, 14, 0, DateTimeKind.Local));
    }

    [Test]
    public void NullWeather_SetsWeatherPropertiesToDefault()
    {
        var result = _mapper.Map<WeatherResponse>(((Weather?)null, CreateValidAstronomy(), CreateValidTimezone()));

        result.TemperatureC.Should().Be(default);
        result.WindKph.Should().Be(default);
        result.FeelsLikeC.Should().Be(default);
        result.PrecipitationMm.Should().Be(default);
    }

    [Test]
    public void NullAstronomy_SetsAstronomyPropertiesToDefault()
    {
        var result = _mapper.Map<WeatherResponse>((CreateValidWeather(), (Astronomy?)null, CreateValidTimezone()));

        result.Moonrise.Should().Be(default);
        result.Moonset.Should().Be(default);
        result.Sunrise.Should().Be(default);
        result.Sunset.Should().Be(default);
        result.MoonIllumination.Should().Be(default);
        result.MoonPhase.Should().Be(default);
    }

    [Test]
    public void NullTimezone_SetsTimezonePropertiesToDefault()
    {
        var result = _mapper.Map<WeatherResponse>((CreateValidWeather(), CreateValidAstronomy(), (Timezone?)null));

        result.LocalTime.Should().Be(default);
        result.Location.Should().Be(default);
        result.TimeZone.Should().Be(default);
    }
}
