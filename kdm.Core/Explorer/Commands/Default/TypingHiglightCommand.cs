using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand]
    public class TypingHiglightCommand : ExplorerCommand
    {
        private const double _typingIntervalThreashold = 0.5;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            var typedCharacted = parameter as string;
            if (typedCharacted == null) return;

            var now = DateTimeOffset.Now;
            var lastTypedDate = Model.InternalState.LastTypedCharacterDate;

            if ((now - lastTypedDate).TotalSeconds > _typingIntervalThreashold)
            {
                Model.InternalState.TypedText = typedCharacted;
            }
            else
            {
                Model.InternalState.TypedText += typedCharacted;
            }

            Model.InternalState.LastTypedCharacterDate = now;

            var elem = Model.ViewState.ExplorerItems
                .Where(x => x.Name.StartsWith(Model.InternalState.TypedText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (elem != null)
            {
                Model.ViewState.SelectedItem = elem;
            }

            await Task.FromResult(0);
        }
    }
}