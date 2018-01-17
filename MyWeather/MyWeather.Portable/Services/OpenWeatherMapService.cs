using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyWeather.Portable.Models;
using Newtonsoft.Json.Serialization;

namespace MyWeather
{
    public class OpenWeatherMapService
    {
        // TO DO: Get your own API key from http://openweathermap.org, don't use mine
        private const string APIKey = "ecedf9b76360a2b876759f6ee85666ba";
        // URI used to get basic weather data. The API key is optional but your request may get denied
        // if you do not include one.
        private const string APIUrl = "http://api.openweathermap.org/data/2.5/weather?q={0}&units=Metric&APPID=" + APIKey;

        /// <summary>
        /// Method that returns a WeatherRoot data object for a specific location from OpenWeatherMap.org
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<WeatherRoot> GetWeather(string location)
        {
            var client = new HttpClient();
            var url = string.Format(APIUrl, location);
            var json = await client.GetStringAsync(url);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            // Deserialize the JSON results into a WeatherRoot object using JSON.NET
            var result = JsonConvert.DeserializeObject<WeatherRoot>(json, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return result;
        }
    }
}
