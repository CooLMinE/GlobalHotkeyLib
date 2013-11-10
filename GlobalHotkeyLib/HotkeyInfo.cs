using System.Windows.Forms;

namespace GlobalHotkeyLib
{
    public class HotkeyInfo
    {
        // The registered hotkey.
        public Keys Key { get; private set; }
        // The first modifier of the hotkey.
        public HotkeyModifiersKeys ModifierKey { get; private set; }
        // The second modifier of the hotkey.
        public HotkeyModifiersKeys ModifierKey2 { get; private set; }

        public HotkeyInfo(Keys key) : this(key, HotkeyModifiersKeys.None, HotkeyModifiersKeys.None) { }

        public HotkeyInfo(Keys key, HotkeyModifiersKeys modifierKey) : this(key, modifierKey, HotkeyModifiersKeys.None) { }

        public HotkeyInfo(Keys key, HotkeyModifiersKeys modifierKey, HotkeyModifiersKeys modifierKey2)
        {
            this.Key = key;
            this.ModifierKey = modifierKey;
            this.ModifierKey2 = modifierKey2;
        }
    }
}
