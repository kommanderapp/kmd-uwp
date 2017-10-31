using System;

namespace kmd.Core.Hotkeys
{
    public class CharReceivedEventArgs : EventArgs
    {
        public CharReceivedEventArgs(string character)
        {
            Character = character;
        }

        public string Character { get; }
        public bool Handled { get; set; }
    }
}