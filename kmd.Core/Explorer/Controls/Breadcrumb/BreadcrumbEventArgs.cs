using System;

namespace kmd.Core.Explorer.Controls
{
    public class BreadcrumbEventArgs : EventArgs
    {
        public BreadcrumbEventArgs(object item, int index)
        {
            this.Item = item;
            this.ItemIndex = index;
        }

        public object Item { get; }
        public object ItemIndex { get; }
    }
}