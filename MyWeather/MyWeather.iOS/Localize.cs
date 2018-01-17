using Foundation;
using MyWeather.Portable.Interfaces;
using Xamarin.Forms;
[assembly: Dependency(typeof(MyWeather.iOS.Localize))]
namespace MyWeather.iOS
{
    public class Localize : ILocalize
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var prefLanguage = "en-US";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-");
            }
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch
            {
                ci = new System.Globalization.CultureInfo(prefLanguage);
            }
            return ci;
        }
    }
}