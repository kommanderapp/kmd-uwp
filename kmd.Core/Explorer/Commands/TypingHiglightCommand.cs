using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System;
using System.Linq;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class TypingHiglightCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var now = DateTimeOffset.Now;
            var lastTypedChar = vm.LastTypedChar;
            var lastTypedDate = vm.LastTypedCharacterDate;

            if ((now - lastTypedDate).TotalSeconds > TypingIntervalThreashold)
            {
                vm.TypedText = lastTypedChar;
            }
            else
            {
                vm.TypedText += lastTypedChar;
            }

            vm.LastTypedCharacterDate = now;

            var elem = vm.ExplorerItems
                .FirstOrDefault(x => x.Name.StartsWith(vm.TypedText, StringComparison.OrdinalIgnoreCase));
            if (elem != null)
            {
                vm.SelectedItem = elem;
            }
        }

        private const double TypingIntervalThreashold = 0.5;
    }
}