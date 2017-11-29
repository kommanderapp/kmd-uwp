using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.Comparer
{
    public class WebViewExtensions
    {
        // "HtmlString" attached property for a WebView
        public static readonly DependencyProperty HtmlStringProperty =
           DependencyProperty.RegisterAttached("HtmlString", typeof(string), typeof(WebViewExtensions), new PropertyMetadata("", OnHtmlStringChanged));

        // Getter and Setter
        public static string GetHtmlString(DependencyObject obj) { return (string)obj.GetValue(HtmlStringProperty); }

        public static void SetHtmlString(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlStringProperty, value);
        }

        // Handler for property changes in the DataContext : set the WebView
        private static void OnHtmlStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebView wv)
            {
                wv.NavigateToString((string)e.NewValue);
            }
        }
    }
}
