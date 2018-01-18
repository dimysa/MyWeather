using MyWeather.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyWeather.Portable.PageModels
{
    public class InfoPageModel: FreshMvvm.FreshBasePageModel
    {
        public InfoModel InfoModel { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }

        public ICommand BackCommand => new Command(async () => await ExecuteBackAsync());

        public override void Init(object initData)
        {
            base.Init(initData);

            InfoModel = initData as InfoModel;
            Sunrise = UnixTimeStampToDateTime(InfoModel.WeatherRoot.Sys.Sunrise).ToString("t");
            Sunset = UnixTimeStampToDateTime(InfoModel.WeatherRoot.Sys.Sunset).ToString("t");
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public async Task ExecuteBackAsync()
        {
            await CoreMethods.PopPageModel();
        }

    }
}
