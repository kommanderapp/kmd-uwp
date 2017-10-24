using Windows.ApplicationModel.Resources;

namespace kmd.Core.Helpers
{
    public static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            var localizedString = _resLoader.GetString(resourceKey);
            if (string.IsNullOrEmpty(localizedString))
            {
                return resourceKey;
            }
            else
            {
                return localizedString;
            }
        }

        private static readonly ResourceLoader _resLoader = new ResourceLoader();
    }
}