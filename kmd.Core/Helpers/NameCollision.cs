using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using System;

namespace kmd.Core.Helpers
{
    public static class NameCollision
    {
        public static string GetUniqueNameForFile(string desiredName, string extenison, IEnumerable<IStorageItem> items)
        {
            if (string.IsNullOrEmpty(desiredName)) throw new ArgumentException(nameof(desiredName));
            if (string.IsNullOrEmpty(extenison)) throw new ArgumentException(nameof(extenison));
            if (items == null || items.Count() == 0) throw new ArgumentException(nameof(items));

            if (!items.Any(i => i.Name == desiredName + extenison))
                return desiredName;

            if (!desiredName.EndsWith(" - Copy"))
                desiredName += " - Copy";
            var c = 1;

            while (items.Any(it => it.Name == desiredName + extenison))
            {
                if (c == 1)
                    desiredName = desiredName.Replace(" - Copy", $" - Copy ({c})");
                else
                    desiredName = desiredName.Replace($" - Copy ({c - 1})", $" - Copy ({c})");

                c++;
            }

            return desiredName + extenison;
        }
        public static string GetUniqueNameForFolder(string desiredName, IEnumerable<IStorageItem> items)
        {
            if (string.IsNullOrEmpty(desiredName)) throw new ArgumentException(nameof(desiredName));
            if (items == null || items.Count() == 0) throw new ArgumentException(nameof(items));

            if (!items.Any(i => i.IsOfType(StorageItemTypes.Folder) && i.Name == desiredName))
                return desiredName;

            if (!desiredName.EndsWith(" - Copy"))
                desiredName += " - Copy";

            var c = 1;

            while (items.Any(i => i.IsOfType(StorageItemTypes.Folder) && i.Name == desiredName))
            {
                if (c == 1)
                    desiredName = desiredName.Replace(" - Copy", $" - Copy ({c})");
                else
                    desiredName = desiredName.Replace($" - Copy ({c - 1})", $" - Copy ({c})");

                c++;
            }

            return desiredName;
        }
    }
}
