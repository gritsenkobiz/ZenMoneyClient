using System;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class BooleanGenericConverter<T> : IValueConverter
    {

        public T TrueValue { get; set; }
        public T FalseValue { get; set; }


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