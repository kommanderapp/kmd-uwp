using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kmd.Core.Explorer
{
    public static class ExplorerManager
    {
        public static ExplorerControl Current => _explorers
            .FirstOrDefault(x => x.Value.IsInFocus).Value;

        public static void Register(ExplorerControl explorerControl)
        {
            var id = Interlocked.Increment(ref _explorerCounter);
            explorerControl.ExplorerId = id;
            _explorers.Add(id, explorerControl);
        }

        public static void Unregister(ExplorerControl explorerControl)
        {
            var id = explorerControl.ExplorerId;
            _explorers.Remove(id);
        }

        private static int _explorerCounter = 0;
        private static Dictionary<int, ExplorerControl> _explorers = new Dictionary<int, ExplorerControl>();
    }
}