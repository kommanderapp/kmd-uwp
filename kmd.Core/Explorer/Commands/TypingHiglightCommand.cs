using kmd.Core.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            if ((now - lastTypedDate).TotalSeconds > _typingIntervalThreashold)
            {
                vm.TypedText = lastTypedChar;
            }
            else
            {
                vm.TypedText += lastTypedChar;
            }

            vm.LastTypedCharacterDate = now;

            var elem = vm.ExplorerItems
                .Where(x => x.Name.StartsWith(vm.TypedText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (elem != null)
            {
                vm.SelectedItem = elem;
            }
        }

        private const double _typingIntervalThreashold = 0.5;
    }
}