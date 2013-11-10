using System;

namespace GlobalHotkeyLib
{
    public class HotkeyPressedEventArgs : EventArgs
    {
        public int Id { get; private set; }
        public HotkeyInfo HotKeyInfo { get; private set; }

        public HotkeyPressedEventArgs(int id, HotkeyInfo hotkeyInfo)
        {            
            this.Id = id;
            this.HotKeyInfo = hotkeyInfo;
        }
    }
}
