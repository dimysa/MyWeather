using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace MyWeather.Portable.Interfaces
{

    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
