using System;
using kmd.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using kmd.Core.Helpers;

namespace kmd.Views
{
    public sealed partial class LocationsPage : Page
    {
        private IStorageFolder _selectedItem;
        public LocationsPage()
        {
            this.InitializeComponent();
        }

        private LocationsViewModel ViewModel
        {
            get { return DataContext as LocationsViewModel; }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }

        private async void NewLocation_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PickLocationAsync();
        }

        private async void DeleteMenuFlyout_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null) await ViewModel.RemoveLocationAsync(_selectedItem);
        }

        private void MenuFlyout_Opening(object sender, object e)
        {
            MenuFlyout senderAsMenuFlyout = sender as MenuFlyout;
            ListViewItem itemContainer = senderAsMenuFlyout.Target as ListViewItem;
            var currentItem = locationsListView.ItemFromContainer(itemContainer);

            if (currentItem is IStorageFolder storageFolder)
            {
                _selectedItem = storageFolder;
            }
        }

        private async void DeleteSwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            if (args.SwipeControl.DataContext is IStorageFolder storageFolder)
            {
                await ViewModel.RemoveLocationAsync(storageFolder);
            }
        }

        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button deleteButton && deleteButton.DataContext is IStorageFolder storageFolder)
            {
                await ViewModel.RemoveLocationAsync(storageFolder);
            }
        }

        private void locationsListView_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                return;
            }

            ListViewItem containerItem = new ListViewItem();
                        
            containerItem.PointerEntered += ContainerItem_PointerEntered;
            containerItem.PointerExited += ContainerItem_PointerExited;

            args.ItemContainer = containerItem;
        }

        private void ContainerItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Touch)
            {
                var item = sender as ListViewItem;
                var deleteButton = item.GetVisualChildByName<Button>("DeleteButton");

                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ContainerItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Touch)
            {
                var item = sender as ListViewItem;
                var deleteButton = item.GetVisualChildByName<Button>("DeleteButton");

                deleteButton.Visibility = Visibility.Visible;
            }
        }
    }
}
