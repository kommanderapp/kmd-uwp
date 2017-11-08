using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Helpers
{
    public static class VisualTreeExtensions
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

        public static T GetVisualChildByName<T>(this FrameworkElement root, string name)
            where T : FrameworkElement
        {
            var chil = VisualTreeHelper.GetChild(root, 0);
            FrameworkElement child = null;

            int count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count && child == null; i++)
            {
                var current = (FrameworkElement)VisualTreeHelper.GetChild(root, i);
                if (current != null && current.Name != null && current.Name == name)
                {
                    child = current;
                    break;
                }
                else
                {
                    child = current.GetVisualChildByName<FrameworkElement>(name);
                }
            }

            return child as T;
        }
    }
}
