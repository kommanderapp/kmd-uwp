using kmd.Storage.Contracts;
using System;
using Windows.UI.Xaml.Data;

namespace kmd.Helpers.Converters
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = (bool)value;
            if (boolValue)
            {
                return 1D;
            }
            else
            {
                return 0D;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
