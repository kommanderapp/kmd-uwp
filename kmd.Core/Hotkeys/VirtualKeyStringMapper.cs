using System;
using System.Collections.Generic;
using Windows.System;

namespace kmd.Core.Hotkeys
{
    public static class VirtualKeyStringMapper
    {
        private static Dictionary<string, string> _virtualKeyStringMap = new Dictionary<string, string>()
        {
            { "192", "`"},
            { "189", "-"},
            { "187", "="},
            { "219", "["},
            { "221", "]"},
            { "220", "\\"},
            { "186", ";"},
            { "222", "'"},
            { "188", ","},
            { "190", "."},
            { "191", "/"},
        };
        public static string ToStringRepresentation(this VirtualKey key)
        {
            var stringRepresentation = string.Empty;
            stringRepresentation = Enum.GetName(typeof(VirtualKey), key);

            if (string.IsNullOrWhiteSpace(stringRepresentation))
            {
                _virtualKeyStringMap.TryGetValue(key.ToString(), out stringRepresentation);
            }

            return stringRepresentation;
        }
    }
}
