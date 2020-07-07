using Xunit;
using consumer_lib;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using FluentAssertions.Collections;
using FluentAssertions;
using System.Collections.Generic;

namespace consumer_tests
{
    public class WeatherForecastClientTest
    {
        [Fact]
        public async void Can_Get_Weather_Forecast_Data()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"date\":\"2020-07-08T13:19:02.5294514+10:00\",\"temperatureC\":-8,\"temperatureF\":18,\"summary\":\"Hot\"}]"),
                })
                .Verifiable();
            handlerMock.Protected().Setup("Dispose", ItExpr.IsAny<bool>());
            var httpClient = new HttpClient(handlerMock.Object);

            var forecastClient = new WeatherForecastClient();
            List<WeatherForecast> result = await forecastClient.GetForecasts("http://localhost:5000", httpClient);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].date.Should().BeSameDateAs(new System.DateTime(2020, 7, 8));
            result[0].temperatureC.Should().BeInRange(-100, 100);
            result[0].temperatureF.Should().BeInRange(-200, 200);
            result[0].summary.Should().NotBeEmpty();
        }
    }
}
