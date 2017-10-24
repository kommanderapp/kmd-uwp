using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Helpers
{
    public static class DependencyObjectExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null) return null;
                if (parentObject is T parent) return parent;
                child = parentObject;
            }
        }
    }
}