using System;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class KilometersToDistanceFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var km = (double) value;

            if (km >= 1)
            {
                return km.ToString("F1") + "км";
            }

            return Math.Round(km*1000).ToString("####") + "м";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return 0.0;
        }
    }
}