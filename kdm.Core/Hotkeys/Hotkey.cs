using System;
using Windows.System;

namespace kmd.Core.Hotkeys
{
    [Flags]
    public enum ModifierKeys
    {
        None,
        Control,
        Shift,
        Alt
    }

    public class Hotkey : IEquatable<Hotkey>
    {
        public static Hotkey For(ModifierKeys modifierKey, VirtualKey key)
        {
            if (modifierKey == ModifierKeys.None && key == VirtualKey.None)
            {
                throw new Exception("Hotkey cant be None + None.");
            }
            var hotkey = new Hotkey(modifierKey, key);
            return hotkey;
        }

        private Hotkey(ModifierKeys modifierKey, VirtualKey key)
        {
            ModifierKey = modifierKey;
            Key = key;
        }

        public ModifierKeys ModifierKey { get; }
        public VirtualKey Key { get; }

        public bool Equals(Hotkey other)
        {
            return this.ModifierKey == other.ModifierKey && this.Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Hotkey;
            if (other == null) throw new InvalidOperationException();
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ModifierKey.GetHashCode() * 13 + Key.GetHashCode() * 13;
        }

        public static bool operator ==(Hotkey a, Hotkey b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Hotkey a, Hotkey b)
        {
            return !(a == b);
        }
    }
}