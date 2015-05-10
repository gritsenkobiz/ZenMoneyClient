using System;
using Windows.UI.Xaml.Data;

namespace Gritsenko.Universal.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            var input = (bool)value;
            return !input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}