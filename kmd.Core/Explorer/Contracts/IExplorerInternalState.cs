using kmd.Core.Explorer.Models;
using kmd.Storage.Contracts;
using System;
using System.Threading;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerInternalState
    {
        CancellationTokenSource CancellationTokenSource { get; set; }
        FilterOptions FilterOptions { get; set; }
        ExplorerItemsStates ItemsState { get; set; }
        string LastTypedChar { get; set; }
        DateTimeOffset LastTypedCharacterDate { get; set; }
        ExplorerNavigationHistory NavigationHistory { get; set; }
        IExplorerItem SelectedItemBeforeExpanding { get; set; }
        string TypedText { get; set; }
    }
}