using System.Threading.Tasks;
using System;
using kmd.Core.Services.Contracts;
using kmd.Core.Explorer.Controls.ContentDialogs;

namespace kmd.Core.Services.Impl
{
    public class PromptService : IPromptService
    {
        public async Task<string> Prompt(string title, string buttonContent, string initialValue = null)
        {
            var dialog = new TextInputDialog { Text = initialValue, Title = title, PrimaryButtonText = buttonContent};
            var result = await dialog.ShowAsync();

            return result == Windows.UI.Xaml.Controls.ContentDialogResult.Primary ? dialog.Text : null;
        }
    }
}
