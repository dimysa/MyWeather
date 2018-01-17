
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MyWeather.Portable.Controls;
using MyWeather.Android.Renderers;
using Android.Content;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace MyWeather.Android.Renderers
{
    class RoundedButtonRenderer: ButtonRenderer
    {
        public RoundedButtonRenderer(Context context): base(context)
        {

        }

        protected override void OnDraw(Canvas canvas)
        {            
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            Control.SetPadding(0, 0, 0, 0);
        }
    }
}