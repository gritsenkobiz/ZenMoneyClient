using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class StringToObjectConverter : IValueConverter
    {
        private ResourceDictionary _values = new ResourceDictionary();

        public object DefaultValue { get; set; }

        public ResourceDictionary Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var key = value.ToString();

            if (Values.ContainsKey(key))
                return Values[key];

            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}