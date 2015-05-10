using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Gritsenko.Universal.Converters
{
    public class NumberSignToBrushBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var input = (double)value;
            if (input == 0)
            {
                return Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
            }
            return new SolidColorBrush(input > 0 ? (Color)Application.Current.Resources["PositiveCountColor"] : (Color)Application.Current.Resources["NegativeCountColor"]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}