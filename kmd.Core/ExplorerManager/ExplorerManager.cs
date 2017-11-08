using kmd.Core.Explorer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kmd.Core.ExplorerManager
{
    public static class ExplorerManager
    {
        public static void Register(ExplorerControl explorerControl)
        {
            var id = Interlocked.Increment(ref _explorerCounter);

            explorerControl.ExplorerId = id;

            _explorers.TryAdd(id, explorerControl);
        }

        public static void Unregister(ExplorerControl explorerControl)
        {
            var id = explorerControl.ExplorerId;

            _explorers.TryRemove(id, out ExplorerControl outValue);
        }

        private static int _explorerCounter = 0;

        private static ConcurrentDictionary<int, ExplorerControl> _explorers = new ConcurrentDictionary<int, ExplorerControl>();
    }
}
