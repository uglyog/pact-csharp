using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace consumer_tests
{
  public class WeatherForecastPact : IDisposable
  {
    public IPactBuilder PactBuilder { get; private set; }
    public IMockProviderService MockProviderService { get; private set; }

    public int MockServerPort { get { return 9222; } }
    public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

    public WeatherForecastPact()
    {
      PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });
      PactBuilder
      .ServiceConsumer("WeatherForcastConsumer")
      .HasPactWith("WeatherForcastAPI");

      MockProviderService = PactBuilder.MockService(MockServerPort);
    }

    public void Dispose()
    {
      PactBuilder.Build();
    }
  }  
}
