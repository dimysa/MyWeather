using System;
using System.Collections.Generic;
using System.Text;
using FreshMvvm;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using MyWeather.Portable.Models;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using System.Linq;

namespace MyWeather.Portable.PageModels
{
    public class WeatherPageModel : FreshBasePageModel
    {
        private readonly OpenWeatherMapService openWeatherMapService;

        private Address address;

        public WeatherPageModel()
        {
            openWeatherMapService = new OpenWeatherMapService();
        }

        public ICommand GetTemperatureCommand => new Command(async () => await ExecuteGetTempAsync());
        public ICommand GetTemperatureFromLocationCommand => new Command(async () => await ExecuteGetTempFromLocationAsync());
        public ICommand OpenInfoCommand => new Command(async () => await ExecuteOpenInfoAsync());

        public string Location { get; set; }
        public WeatherRoot WeatherInfo { get; set; }
        public bool IsLoading { get; set; }

        private async Task ExecuteGetTempAsync()
        {
            if (string.IsNullOrEmpty(Location))
            {
                await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.ErrorCity, "Ok");
                return;
            }

            try
            {
                // Call the eather service and await the call
                var wr = await openWeatherMapService.GetWeather(Location);
                if (wr != null)
                {
                    WeatherInfo = wr;

                    // TEXT-TO-SPEECH INTEGRATION
                    string greeter = "";
                    // Customize the speech intro depending on the platformVoi
                    if (Device.RuntimePlatform == Device.iOS)
                        greeter = Resource.Resource.GreetingSiri;
                    if (Device.RuntimePlatform == Device.Android)
                        greeter = Resource.Resource.GreetingDroid;

                    // Build a message string to be spoken out loud
                    string weatherMessageTemplate =  $"{greeter}. {Resource.Resource.VoiceMessage}";
                    string weatherMessage = string.Format(weatherMessageTemplate, greeter, wr.Name, (int)wr.Main.Temp,
                        (int)wr.Main.TempMax, (int)wr.Main.TempMin);
                    // Call the Text-to-Speech Dependency service on each platform to play the weather message with
                    // the platform's speech synthesizer
                    DependencyService.Get<ITextToSpeech>().Speak(weatherMessage);
                }
            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.ErrorInternet, "Ok");
                IsLoading = false;
            }
        }

        public async Task ExecuteGetTempFromLocationAsync()
        {
            IsLoading = true;
            var city = await GetCurrentLocation();
            if (city != null)
            {
                Location = city;
                await ExecuteGetTempAsync();
            }
            IsLoading = false;

        }

        public async Task<string> GetCurrentLocation()
        {
            Position position = null;
            IEnumerable<Address> addresses = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                //position = await locator.GetLastKnownLocationAsync();

                //if (position != null)
                //{
                //    addresses = await locator.GetAddressesForPositionAsync(position);
                //    address = addresses.FirstOrDefault();

                //    if (address == null)
                //    {
                //        await CoreMethods.DisplayAlert("Error", "Address not found", "OK");
                //        return null;
                //    }
                //    else
                //        return address.Locality;
                //}

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.ErrorGeolocation, "OK");
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                if (position == null)
                {
                    await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.ErrorPosition, "OK");
                    return null;
                }

                addresses = await locator.GetAddressesForPositionAsync(position);
                address = addresses.FirstOrDefault();

                if (address == null)
                {
                    await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.ErrorAddress, "OK");
                    return null;
                }
                else                    
                    return address.Locality;
            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert(Resource.Resource.Error, Resource.Resource.UnexpectedError, "OK");
                IsLoading = false;
                return null;
            }
        }

        private async Task ExecuteOpenInfoAsync()
        {
            InfoModel model = new InfoModel
            {
                Address = address,
                WeatherRoot = this.WeatherInfo
            };

            await CoreMethods.PushPageModel<InfoPageModel>(model);
        }
    }
}
