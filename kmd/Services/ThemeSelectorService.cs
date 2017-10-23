using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using kmd.Core.Helpers;

namespace kmd.Services
{
    public static class ThemeSelectorService
    {
        public static event EventHandler<ElementTheme> OnThemeChanged = (sender, args) => { };

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        public static SolidColorBrush GetSystemControlForegroundForTheme()
        {
            return _baseBrush;
        }

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
        }

        public static void SetRequestedTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Theme;
            }
        }

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            SetRequestedTheme();
            await SaveThemeInSettingsAsync(Theme);

            OnThemeChanged(null, Theme);
        }

        private const string SettingsKey = "RequestedTheme";

        private static readonly SolidColorBrush _baseBrush = Application.Current.Resources["ThemeControlForegroundBaseHighBrush"] as SolidColorBrush;

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
