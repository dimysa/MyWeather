using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace MyWeather.Portable.Controls.Extensions
{
    public static class EntryExtensions
    {
        public static T GetInternalField<T>(this BindableObject element, string propertyKeyName) where T : class
        {
            // reflection stinks, but hey, what can you do?
            var pi = element.GetType().GetField(propertyKeyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var key = (T)pi?.GetValue(element);

            return key;
        }
    }
}
