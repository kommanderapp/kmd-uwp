using kmd.Core.Explorer.Controls.ContentDialogs;
using kmd.Core.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace kmd.Core.Services.Impl
{
    public class CustomDialogService : ICustomDialogService
    {
        public async Task<NameCollisionDialogResult> NameCollisionDialog(string filename, bool isForSingleFile = true)
        {
            var dialog = new NameCollisionDialog(filename, isForSingleFile)
            {
                Title = $"The destination already has a file named \"{filename}\""
            };
            var result = await dialog.ShowAsync();

            return dialog.Result;
        }

        public async Task<string> Prompt(string title, string initialValue = null)
        {
            var dialog = new TextInputDialog { Text = initialValue, Title = title, PrimaryButtonText = "Ok" };
            var result = await dialog.ShowAsync();

            return result == Windows.UI.Xaml.Controls.ContentDialogResult.Primary ? dialog.Text : null;
        }
    }
}
