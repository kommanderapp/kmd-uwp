using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Extensions.Converters
{
    public class BooleanToGridLengthConverter : IValueConverter
    {
        public GridLength FalseSize { get; set; } = new GridLength(0);
        public GridLength TrueSize { get; set; } = new GridLength(1, GridUnitType.Star);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolvalue && boolvalue) return TrueSize;
            return FalseSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
