using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherApp.Model;
using WeatherApp.Model.Json;
using WeatherApp.Services;

namespace WeatherApp.Tests.Services;

[TestFixture]
public class WeatherServiceTests
{
    private IWeatherService _service = null!;
    private Mock<HttpMessageHandler> _handlerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"location\":{}, \"current\":{}, \"astronomy\":{ \"astro\": {}}}")
        };

        _handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(response);

        var httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri(Constants.WeatherApiBaseUri),
        };

        var mapper = new Mock<IMapper>();
        var weatherResponse = new WeatherResponse();
        mapper.Setup(map => map.Map<WeatherResponse>(ItExpr.IsAny<(Weather?, Astronomy?, Timezone?)>()))
            .Returns(weatherResponse);

        var logger = new Mock<ILogger<WeatherService>>();

        _service = new WeatherService(httpClient, mapper.Object, logger.Object);
    }
    
    [Test]
    public async Task GetWeatherResponse_CallsWeatherApi()
    {
        await  _service.GetWeatherResponseAsync("Amman");

        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
