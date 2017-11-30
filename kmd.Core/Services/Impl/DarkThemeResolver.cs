using kmd.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace kmd.Core.Services.Impl
{
    public class DarkThemeResolver : IDarkThemeResolver
    {
        public bool IsDarkMode()
        {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            return frameworkElement.ActualTheme == ElementTheme.Dark;
        }
    }
}
