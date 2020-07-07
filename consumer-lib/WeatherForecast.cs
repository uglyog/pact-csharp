using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace consumer_lib
{
    #pragma warning disable CS0649 
    public struct WeatherForecast
    {
        public DateTime date;
        public int temperatureC;
        public int temperatureF;
        public string summary;
    }

    public class WeatherForecastClient
    {
        #nullable enable
        public async Task<List<WeatherForecast>> GetForecasts(string baseUrl, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "/WeatherForecast");
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<WeatherForecast>>(resp);
        }
    }
}
