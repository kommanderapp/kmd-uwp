using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using System;

namespace kmd.Core.Helpers
{
    public static class NameCollision
    {
        public static string GetUniqueNameForFile(string desiredName, string extenison, IEnumerable<IStorageItem> items, string postfix = "")
        {
            if (string.IsNullOrEmpty(desiredName)) throw new ArgumentException(nameof(desiredName));
            if (postfix == null) throw new ArgumentNullException(nameof(postfix));
            if (string.IsNullOrEmpty(extenison)) throw new ArgumentException(nameof(extenison));
            if (items == null || items.Count() == 0) throw new ArgumentException(nameof(items));

            if (!items.Any(i => i.Name == desiredName + extenison))
                return desiredName;

            if (postfix != string.Empty && !desiredName.EndsWith(postfix))
                desiredName += postfix;

            var c = 1;

            while (items.Any(it => it.Name == desiredName + extenison))
            {
                if (c == 1)
                    if (postfix == string.Empty)
                        desiredName += $" ({c})";
                    else
                        desiredName = desiredName.Replace(postfix, $"{postfix} ({c})");
                else
                    desiredName = desiredName.Replace($"{postfix} ({c - 1})", $"{postfix} ({c})");

                c++;
            }

            return desiredName + extenison;
        }
        public static string GetUniqueNameForFolder(string desiredName, IEnumerable<IStorageItem> items, string postfix = "")
        {
            if (string.IsNullOrEmpty(desiredName)) throw new ArgumentException(nameof(desiredName));
            if (postfix == null) throw new ArgumentNullException(nameof(postfix));
            if (items == null || items.Count() == 0) throw new ArgumentException(nameof(items));

            if (!items.Any(i => i.IsOfType(StorageItemTypes.Folder) && i.Name == desiredName))
                return desiredName;

            if (postfix != string.Empty && !desiredName.EndsWith(postfix))
                desiredName += postfix;

            var c = 1;

            while (items.Any(i => i.IsOfType(StorageItemTypes.Folder) && i.Name == desiredName))
            {
                if (c == 1)
                    if (postfix == string.Empty)
                        desiredName += $" ({c})";
                    else
                        desiredName = desiredName.Replace(postfix, $"{postfix} ({c})");
                else
                    desiredName = desiredName.Replace($"{postfix} ({c - 1})", $"{postfix} ({c})");

                c++;
            }

            return desiredName;
        }
    }
}
