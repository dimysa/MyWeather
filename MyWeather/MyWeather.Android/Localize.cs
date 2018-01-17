using MyWeather.Portable.Interfaces;
using Xamarin.Forms;
[assembly: Dependency(typeof(MyWeather.Droid.Localize))]

namespace MyWeather.Droid
{
    public class Localize : ILocalize
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-");
            return new System.Globalization.CultureInfo(netLanguage);
        }
    }
}