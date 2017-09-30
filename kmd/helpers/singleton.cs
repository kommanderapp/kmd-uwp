using System;
using System.Collections.Concurrent;

namespace kmd.Helpers
{
    internal static class Singleton<T>
        where T : new()
    {
        public static T Instance
        {
            get
            {
                return _instances.GetOrAdd(typeof(T), (t) => new T());
            }
        }

        private static ConcurrentDictionary<Type, T> _instances = new ConcurrentDictionary<Type, T>();
    }
}
