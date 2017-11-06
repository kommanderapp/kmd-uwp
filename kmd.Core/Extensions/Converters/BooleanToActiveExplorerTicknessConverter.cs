using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Extensions.Converters
{
    public class BooleanToActiveExplorerTicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && (bool)value == true)
            {
                return new Thickness(1, 1, 1, 3);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
