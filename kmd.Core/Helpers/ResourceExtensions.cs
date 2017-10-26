using Windows.ApplicationModel.Resources;

namespace kmd.Core.Helpers
{
    public static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            var localizedString = ResLoader.GetString(resourceKey);
            return string.IsNullOrEmpty(localizedString) ? resourceKey : localizedString;
        }

        private static readonly ResourceLoader ResLoader = new ResourceLoader();
    }
}