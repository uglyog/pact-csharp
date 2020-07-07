using System;
using System.Threading.Tasks;
using consumer_lib;

namespace consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new WeatherForecastClient();

            var contributors = await client.GetForecasts("http://localhost:5000");
            contributors.ForEach((forcast) => {
                Console.WriteLine(String.Format("{0,-10} {1,10} {2,4}C ({3,4}F)", forcast.date.ToString("ddd dd/MM"), forcast.summary, forcast.temperatureC, forcast.temperatureF));
            });
        }
    }
}
