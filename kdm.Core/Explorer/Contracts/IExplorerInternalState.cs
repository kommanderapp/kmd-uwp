using kmd.Storage.Contracts;
using System;
using System.Threading;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerInternalState
    {
        ExplorerItemsStates ItemsState { get; set; }
        CancellationTokenSource CancellationTokenSource { get; set; }
        IExplorerItem SelectedItemBeforeExpanding { get; set; }
        string TypedText { get; set; }
        DateTimeOffset LastTypedCharacterDate { get; set; }
        FilterOptions FilterOptions { get; set; }
    }
}