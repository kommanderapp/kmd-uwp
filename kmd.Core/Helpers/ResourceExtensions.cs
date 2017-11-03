using Windows.ApplicationModel.Resources;

namespace kmd.Core.Helpers
{
    public static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            if (resourceKey == null) return string.Empty;
            var localizedString = ResLoader.GetString(resourceKey);
            return string.IsNullOrEmpty(localizedString) ? resourceKey : localizedString;
        }

        private static readonly ResourceLoader ResLoader = new ResourceLoader();
    }
}
