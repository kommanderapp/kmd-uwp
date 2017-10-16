using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace kmd.Core.Explorer.Controls.Breadcrumb
{
    public class BreadcrumbDragEventArgs
    {
        public BreadcrumbDragEventArgs(object item, DragEventArgs args)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            DragArgs = args ?? throw new ArgumentNullException(nameof(args));
        }

        public DragEventArgs DragArgs { get; }
        public object Item { get; }
    }
}