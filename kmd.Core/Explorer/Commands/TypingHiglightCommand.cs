using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class TypingHiglightCommand : ExplorerCommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            var typedCharacted = parameter as string;
            if (typedCharacted == null) return;

            var now = DateTimeOffset.Now;
            var lastTypedDate = ViewModel.LastTypedCharacterDate;

            if ((now - lastTypedDate).TotalSeconds > _typingIntervalThreashold)
            {
                ViewModel.TypedText = typedCharacted;
            }
            else
            {
                ViewModel.TypedText += typedCharacted;
            }

            ViewModel.LastTypedCharacterDate = now;

            var elem = ViewModel.ExplorerItems
                .Where(x => x.Name.StartsWith(ViewModel.TypedText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (elem != null)
            {
                ViewModel.SelectedItem = elem;
            }

            await Task.FromResult(0);
        }

        private const double _typingIntervalThreashold = 0.5;
    }
}