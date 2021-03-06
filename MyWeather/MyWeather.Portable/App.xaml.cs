﻿using FreshMvvm;
using MyWeather.Portable.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyWeather.Portable
{
    public partial class App: Application
    {
        public App()
        {
            InitializeComponent();
            if (Device.RuntimePlatform != Device.WinPhone)
            {
                MyWeather.Portable.Resource.Resource.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
            Init();
            var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<MyWeather.Portable.PageModels.WeatherPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        private void Init()
        {

        }
        
        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
