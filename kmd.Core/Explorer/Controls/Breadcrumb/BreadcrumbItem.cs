using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Explorer.Controls.Breadcrumb
{
    internal class BreadcrumbItem : Button
    {
        public BreadcrumbItem()
        {
            Background = new SolidColorBrush(new Color()); // transparent
            BorderThickness = new Thickness(0);
            AllowDrop = true;
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