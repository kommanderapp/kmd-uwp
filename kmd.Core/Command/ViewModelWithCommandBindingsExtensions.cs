using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command
{
    public static class ViewModelWithCommandBindingsExtensions
    {
        public static void ExecuteCommand(this IViewModelWithCommandBindings vm, Type type)
        {
            if (vm == null) throw new ArgumentNullException(nameof(vm));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var command = vm.CommandBindings[type];
            if (command == null)
            {
                throw new InvalidOperationException($"No command regsiterd by type {type}");
            }

            if (command.CanExecute(vm))
            {
                command.Execute(vm);
            }
        }
    }
}