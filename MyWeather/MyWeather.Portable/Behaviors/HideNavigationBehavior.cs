using Xamarin.Forms;

namespace MyWeather.Portable.Behaviors
{
    public class HideNavigationBehavior : Behavior<Page>
    {
        protected override void OnAttachedTo(Page bindable)
        {
            base.OnAttachedTo(bindable);

            NavigationPage.SetHasNavigationBar(bindable, false);
        }
    }
}
