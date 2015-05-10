using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class BooleanToCollapsedVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            var input = (null == parameter) ? (bool)value : !((bool)value);
            return !(input) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
