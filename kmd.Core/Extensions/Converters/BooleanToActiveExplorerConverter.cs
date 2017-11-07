using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Extensions.Converters
{
    public class BooleanToActiveExplorerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && (bool)value == true)
            {
                var accentColor = (Color)Application.Current.Resources["SystemAccentColor"];
                return new SolidColorBrush(accentColor);
            }

            return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
