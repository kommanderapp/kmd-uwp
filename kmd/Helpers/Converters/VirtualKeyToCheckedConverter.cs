using System;
using Windows.System;
using Windows.UI.Xaml.Data;

namespace kmd.Helpers.Converters
{
    public class VirtualKeyToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is VirtualKey selectedKey
                && parameter is string currentKeyString
                && Enum.TryParse(currentKeyString, out VirtualKey enumParam)
                && selectedKey == enumParam)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue
                && boolValue == true
                && parameter is string currentKeyString
                && Enum.TryParse(currentKeyString, out VirtualKey enumParam))
                return enumParam;
            else
                return VirtualKey.None;
        }
    }
}
