using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Gritsenko.Universal.Converters
{
    public class BooleanToBrushConverter : IValueConverter
    {
        public Brush TrueValue { get; set; }
        public Brush FalseValue { get; set; }


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((bool)value) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.Equals(TrueValue);
        }

    }
}