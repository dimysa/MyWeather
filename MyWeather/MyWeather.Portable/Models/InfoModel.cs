using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWeather.Portable.Models
{
    public class InfoModel: BaseModel
    {
        public WeatherRoot WeatherRoot { get; set; }
        public Address Address { get; set; }
    }
}
