using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Extensions.Converters
{
    public class InverseBooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue && boolValue) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue && boolValue) return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
