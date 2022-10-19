using System.Reflection;
using WeatherApp;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200").WithMethods("GET");
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(Constants.WeatherApiBaseUri);
    var key = builder.Configuration["WeatherApiKey"];
    httpClient.DefaultRequestHeaders.Add(Constants.WeatherApiKeyHeaderName, key);
    httpClient.DefaultRequestHeaders.Add(Constants.WeatherApiHostHeaderName, Constants.WeatherApiHostHeaderValue);
});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.MapControllers();
app.Run();
public partial class Program { }
