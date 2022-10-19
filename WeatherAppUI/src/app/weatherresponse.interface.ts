export interface IWeatherResponse {
    timeZone: string;
    location: string;
    localTime: string;
    windKph: number;
    temperatureC: number;
    precipitationMm: number;
    feelsLikeC: number;
    sunrise: Date;
    sunset: Date;
    moonrise: Date;
    moonset: Date;
    moonPhase: string;
    moonIllumination: number;
}