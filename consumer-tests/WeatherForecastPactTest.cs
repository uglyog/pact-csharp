using System.Collections.Generic;
using consumer_lib;
using FluentAssertions;
using PactNet.Matchers;
using PactNet.Matchers.Type;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace consumer_tests
{
  public class WeatherForecastPactTest : IClassFixture<WeatherForecastPact>
  {
    private IMockProviderService mockProviderService;
    private string mockProviderServiceBaseUri;

    public WeatherForecastPactTest(WeatherForecastPact data)
    {
      mockProviderService = data.MockProviderService;
      mockProviderService.ClearInteractions();
      mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
    }

    [Fact]
    public async void Can_Get_Weather_Forecast_Data()
    {
      mockProviderService
      .UponReceiving("A request for the weather forecast")
      .With(new ProviderServiceRequest
      {
        Method = HttpVerb.Get,
        Path = "/WeatherForecast"
      })
      .WillRespondWith(new ProviderServiceResponse
      {
        Status = 200,
        Headers = new Dictionary<string, object>
        {
          { "Content-Type", "application/json; charset=utf-8" }
        },
        Body = new MinTypeMatcher(new
          {
            date = "2020-07-08T13:19:02.5294514+10:00",
            temperatureC = Match.Type(-8),
            temperatureF = Match.Type(18),
            summary = Match.Type("Hot")
          }, 1)
      });

      var consumer = new WeatherForecastClient();
      List<WeatherForecast> result = await consumer.GetForecasts(mockProviderServiceBaseUri);
      result.Should().NotBeNull();
      result.Should().HaveCount(1);
      result[0].date.Should().BeSameDateAs(new System.DateTime(2020, 7, 8));
      result[0].temperatureC.Should().BeInRange(-100, 100);
      result[0].temperatureF.Should().BeInRange(-200, 200);
      result[0].summary.Should().NotBeEmpty();
    }
  }
}
