using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Controls.ContentDialogs;
using kmd.Core.Hotkeys;
using System;
using System.Threading.Tasks;

namespace kmd.Core.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static async Task<NameCollisionDialogResult> NameCollisionDialog(this IDialogService dialogService, string filename, bool isForSingleFile = true)
        {
            KeyEventsAgregator.IsDisabled = true;

            var dialog = new NameCollisionDialog(filename, isForSingleFile)
            {
                Title = $"The destination already has a file named \"{filename}\""
            };
            var result = await dialog.ShowAsync();

            KeyEventsAgregator.IsDisabled = false;

            return dialog.Result;
        }

        public static async Task FileInfo(this IDialogService dialogService, IExplorerItem file)
        {
            KeyEventsAgregator.IsDisabled = true;

            var dialog = new FileInfoDialog(file) { Title = "Details", PrimaryButtonText = "Ok" };
            var result = await dialog.ShowAsync();

            KeyEventsAgregator.IsDisabled = false;
        }

        public static async Task<string> Prompt(this IDialogService dialogService, string title, string initialValue = null)
        {
            KeyEventsAgregator.IsDisabled = true;

            var dialog = new TextInputDialog { Text = initialValue, Title = title, PrimaryButtonText = "Ok" };
            var result = await dialog.ShowAsync();

            KeyEventsAgregator.IsDisabled = false;

            return result == Windows.UI.Xaml.Controls.ContentDialogResult.Primary ? dialog.Text : null;
        }
    }
}
