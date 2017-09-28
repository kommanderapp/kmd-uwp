using System;

namespace kmd.Core.Explorer.Contracts
{
    public class StateChangedArgs : EventArgs
    {
        public ExplorerItemsStates Source { get; private set; }
        public ExplorerItemsStates Destination { get; private set; }

        public StateChangedArgs(ExplorerItemsStates source, ExplorerItemsStates destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}