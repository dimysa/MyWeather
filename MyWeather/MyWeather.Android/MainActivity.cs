using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyWeather.Portable;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content.PM;
using Plugin.Permissions;

namespace MyWeather.Droid
{
    [Activity(Label = "MyWeather", MainLauncher = true, Theme = "@style/MainTheme")]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }   
}

