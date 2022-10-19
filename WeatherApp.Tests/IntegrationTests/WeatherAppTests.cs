using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using WeatherApp.Model;

namespace WeatherApp.Tests.IntegrationTests;

[TestFixture]
public class WeatherAppTests
{
    [Test]
    public async Task EndpointReturnsSuccessResponse()
    {
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        var result = await client.GetAsync("/Weather?cityName=Cairo");

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().Contain("temperatureC");
        content.Should().Contain("sunrise");
        content.Should().Contain("location");
    }
}