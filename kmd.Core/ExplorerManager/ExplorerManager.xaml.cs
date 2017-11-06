using kmd.Core.Command;
using kmd.Core.Explorer;
using kmd.Core.Explorer.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kmd.Core.ExplorerManager
{
    public sealed partial class ExplorerManager : UserControl
    {
        public ExplorerManager()
        {
            this.InitializeComponent();
        }

        private void NavigateBackward_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateBackwardCommand));
        }

        private void NavigateForward_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateForwardCommand));
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(CopySelectedItemCommand));
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(CutSelectedItemCommand));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(DeleteSelectedItemCommand));
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(PasteToCurrentFolderCommand));
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(ExplodeCurrentFolderCommand));
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(ItemPathToClipboardCommand));
        }

        private void NavigateUp_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateToParrentCommand));
        }

        private void SortMethod_Changed(object sender, RoutedEventArgs e)
        {
            if (Current?.ViewModel?.ExplorerItems == null) return;
            var item = (RadioButton)sender;

            var method = Enum.GetValues(typeof(SortMethod)).Cast<SortMethod>().First(o => o.ToString().Equals(item.Name));
            Current.ViewModel.Sort(method);
        }

        public static ExplorerControl Current => _explorers.FirstOrDefault(x => x.Value.ItemsInFocus).Value;

        public static void Register(ExplorerControl explorerControl)
        {
            var id = Interlocked.Increment(ref _explorerCounter);
            explorerControl.ExplorerId = id;
            _explorers.TryAdd(id, explorerControl);
        }

        public static void Unregister(ExplorerControl explorerControl)
        {
            var id = explorerControl.ExplorerId;
            _explorers.TryRemove(id, out ExplorerControl outValue);
        }

        private static int _explorerCounter = 0;
        private static ConcurrentDictionary<int, ExplorerControl> _explorers = new ConcurrentDictionary<int, ExplorerControl>();
    }
}
