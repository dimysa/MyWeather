using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MyWeather.Portable.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Coord: BaseModel
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class Sys: BaseModel
    {
        public double Message { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }

    public class Weather: BaseModel
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    /// <summary>
    /// Main weather data
    /// </summary>
    public class Main: BaseModel
    {
        public double Temp { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double Pressure { get; set; }
        public double SeaLevel { get; set; }
        public double GrndLevel { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind: BaseModel
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
    }

    public class Clouds: BaseModel
    {
        public int All { get; set; }        
    }

    public class WeatherRoot: BaseModel
    {
        public Coord Coord { get; set; }
        public Sys Sys { get; set; }
        public List<Weather> Weather { get; set; }
        public string @base { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public int Dt { get; set; }
        public int Ld { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }        
    }
}

