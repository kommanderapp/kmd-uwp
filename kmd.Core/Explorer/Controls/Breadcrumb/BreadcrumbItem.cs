using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Explorer.Controls
{
    internal class BreadcrumbItem : Button
    {
        public BreadcrumbItem()
        {
            Background = new SolidColorBrush(new Color()); // transparent
            BorderThickness = new Thickness(0);
            IsTabStop = false;
        }
    }

    internal class BreadcrumbSeperator : ContentControl
    {
        public BreadcrumbSeperator()
        {
            IsTabStop = false;
        }
    }
}