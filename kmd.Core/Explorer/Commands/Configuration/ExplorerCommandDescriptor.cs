using System;
using kmd.Core.Hotkeys;
using kmd.Core.Command.Configuration;
using kmd.Core.Helpers;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandDescriptor : CommandDescriptor
    {
        public ExplorerCommandDescriptor(Type type, ExplorerCommandAttribute attribute)
        {
            CommandType = type ?? throw new ArgumentNullException(nameof(type));
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            DefaultHotkey = Attribute.DefaultHotkey;
            UniqueName = attribute.UniqueName;
            Description = attribute.DescriptionResKey.GetLocalized();
        }

        public ExplorerCommandAttribute Attribute { get; }
        public Type CommandType { get; }
    }
}
