using System;
using System.Text;

namespace kmd.Core.Helpers
{
    public static class Unicode
    {
        public static string ToString(uint val)
        {
            var bytes = BitConverter.GetBytes(val);

            if (bytes == null || bytes.Length <= 0) return string.Empty;

            return Encoding.Unicode.GetString(bytes)[0].ToString();
        }
    }
}