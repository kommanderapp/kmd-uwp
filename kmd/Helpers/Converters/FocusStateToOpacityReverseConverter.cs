using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace kmd.Helpers.Converters
{
    public class FocusStateToOpacityReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = (FocusState)value;
            if (state != FocusState.Unfocused)
            {
                // if control has focus
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
