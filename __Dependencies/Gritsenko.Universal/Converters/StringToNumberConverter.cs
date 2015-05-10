using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class StringToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var strValue = value.ToString();

            return Parse(strValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof (double))
            {
                return Parse(value.ToString());
            }

            return value.ToString();
        }

        public static double Parse(string value)
        {
            double sum;

            double.TryParse(value.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out sum);
            return sum;
        }

    }
}