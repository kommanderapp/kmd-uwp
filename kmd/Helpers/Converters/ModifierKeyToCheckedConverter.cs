using kmd.Core.Hotkeys;
using System;
using Windows.UI.Xaml.Data;

namespace kmd.Helpers.Converters
{
    public class ModifierKeyToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ModifierKeys selectedKey
                && parameter is string currentKeyString
                && Enum.TryParse(currentKeyString, out ModifierKeys enumParam)
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
                && Enum.TryParse(currentKeyString, out ModifierKeys enumParam))
                return enumParam;
            else
                return ModifierKeys.None;
        }        
    }
}
