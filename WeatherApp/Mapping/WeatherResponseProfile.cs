using System.Globalization;
using System.Text.RegularExpressions;
using AutoMapper;
using WeatherApp.Model.Json;

namespace WeatherApp.Mapping;

public class WeatherResponseProfile : Profile
{
    public WeatherResponseProfile()
    {
        CreateMap<(Weather weather, Astronomy astronomy, Timezone timezone), Model.WeatherResponse>()
            .ForMember(d => d.Location, opt => opt.MapFrom(s => s.timezone.Name))
            .ForMember(d => d.TimeZone, opt => opt.MapFrom(s => s.timezone.TimezoneId))
            .ForMember(d => d.LocalTime, opt => opt.MapFrom(s => ParseLocalTime(s.timezone.LocalTime ?? string.Empty)))
            .ForMember(d => d.Moonrise,
                opt => opt.MapFrom(s => GetAstronomyTimeProperty(s.astronomy, s.timezone, a => a.Moonrise)))
            .ForMember(d => d.Moonset,
                opt => opt.MapFrom(s => GetAstronomyTimeProperty(s.astronomy, s.timezone, a => a.Moonset)))
            .ForMember(d => d.Sunrise,
                opt => opt.MapFrom(s => GetAstronomyTimeProperty(s.astronomy, s.timezone, a => a.Sunrise)))
            .ForMember(d => d.Sunset,
                opt => opt.MapFrom(s => GetAstronomyTimeProperty(s.astronomy, s.timezone, a => a.Sunset)))
            .ForMember(d => d.MoonIllumination, opt => opt.MapFrom(s => s.astronomy.MoonIllumination))
            .ForMember(d => d.MoonPhase, opt => opt.MapFrom(s => s.astronomy.MoonPhase))
            .ForMember(d => d.TemperatureC, opt => opt.MapFrom(s => s.weather.TemperatureC))
            .ForMember(d => d.WindKph, opt => opt.MapFrom(s => s.weather.WindKph))
            .ForMember(d => d.FeelsLikeC, opt => opt.MapFrom(s => s.weather.FeelsLikeC))
            .ForMember(d => d.PrecipitationMm, opt => opt.MapFrom(s => s.weather.PrecipitationMm));
    }

    private static DateTime? GetAstronomyTimeProperty(Astronomy? astronomy, Timezone? timezone, Func<Astronomy, string?> selectProperty)
    {
        if (astronomy == null || timezone == null) return null;

        var localTime = ParseLocalTime(timezone.LocalTime!);
        var propertyValue = selectProperty(astronomy);

        if (string.IsNullOrEmpty(propertyValue)) return null;

        var expectedPattern = new Regex(@"\d\d:\d\d (AM|PM)");
        if (!expectedPattern.IsMatch(propertyValue)) return null;

        var hourString = propertyValue.Substring(0, 2);
        var amOrPm = propertyValue.Substring(6, 2);
        var hour = int.Parse(hourString) + (amOrPm == "PM" ? 12 : 0);

        var minuteString = propertyValue.Substring(3, 2);
        var minute = int.Parse(minuteString);

        return new DateTime(localTime.Year, localTime.Month, localTime.Day, hour, minute, 0,
            DateTimeKind.Local);
    }

    private static DateTime ParseLocalTime(string localTimeString)
    {
        DateTime.TryParseExact(localTimeString, "yyyy-MM-dd H:mm", CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal, out DateTime dateTime);
        DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        return dateTime;
    }
}