using GlobalHotkeyLib.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlobalHotkeyLib
{
    public class GlobalHotkeyHandler : IDisposable
    {
        private volatile bool isDisposing = false;
        private int hotkeyId;
        private HotkeyHandle windowHandle;
        public delegate void KeyEventHandler(object sender, HotkeyPressedEventArgs e);
        public event KeyEventHandler HotkeyPressed;
        private Dictionary<int, HotkeyInfo> registeredHotkeys;

        public GlobalHotkeyHandler()
        {
            this.hotkeyId = 0;
            this.registeredHotkeys = new Dictionary<int, HotkeyInfo>();
            this.windowHandle = new HotkeyHandle(this);
            this.windowHandle.HotkeyMessageReceived += windowHandle_HotkeyMessageReceived;
        }

        /// <summary>
        /// Triggers when the HotkeyHandle receives a WM_HOTKEY message. 
        /// </summary>
        void windowHandle_HotkeyMessageReceived(object sender, HotkeyPressedEventArgs e)
        {
            HotkeyPressed(this, e);
        }

        /// <summary>
        /// Get the hotkey information of a registered hotkey.
        /// </summary>
        /// <param name="id">The id of the hotkey.</param>
        /// <returns>If the hotkey is registered it returns a HotkeyInfo object that contains the hotkey and the hotkey modifiers. 
        /// Throws a <b>KeyNotFoundException</b> exception if no hotkey with that id is registered.</returns>
        public HotkeyInfo GetHotkeyInfoById(int id)
        {
            foreach (var entry in registeredHotkeys)
            {
                if (entry.Key == id)
                    return entry.Value;
            }
            throw new KeyNotFoundException("No hotkey with that id is registered");
        }

        /// <summary>
        /// Register hotkey.
        /// </summary>
        /// <param name="key">The key to register as hotkey.</param>
        public void RegisterHotkey(Keys key)
        {
            this.RegisterHotkey(key, HotkeyModifiersKeys.None, HotkeyModifiersKeys.None);
        }

        /// <summary>
        /// Register hotkey.
        /// </summary>
        /// <param name="key">The key to register as hotkey.</param>
        /// <param name="modifier">The modifier for the hotkey.</param>
        public void RegisterHotkey(Keys key, HotkeyModifiersKeys modifier)
        {
            this.RegisterHotkey(key, modifier, HotkeyModifiersKeys.None);
        }

        /// <summary>
        /// Register hotkey.
        /// </summary>
        /// <param name="key">The key to register as hotkey.</param>
        /// <param name="modifier">The modifier for the hotkey.</param>
        /// <param name="modifier2">The second modifier for the hotkey.</param>
        public void RegisterHotkey(Keys key, HotkeyModifiersKeys modifier, HotkeyModifiersKeys modifier2)
        {
            if (!NativeMethods.RegisterHotKey(this.windowHandle.Handle, hotkeyId, (int)modifier | (int)modifier2, key.GetHashCode()))
                throw new InvalidOperationException("Could not register hotkey. Error code: " + NativeMethods.GetLastError());

            registeredHotkeys.Add(hotkeyId, new HotkeyInfo(key, modifier, modifier2));
            hotkeyId++;
        }


        /// <summary>
        /// Unregister a hotkey.
        /// </summary>
        /// <param name="hotkeyId">The id of the hotkey to unregister.</param>
        public void UnregisterHotkey(int hotkeyId)
        {
            var tempList = new Dictionary<int, HotkeyInfo>(registeredHotkeys);

            foreach (var entry in tempList)
            {
                if (entry.Key == hotkeyId)
                {
                    NativeMethods.UnregisterHotKey(this.windowHandle.Handle, entry.Key);
                    registeredHotkeys.Remove(entry.Key);
                }
            }
        }

        /// <summary>
        /// Unregister a hotkey.
        /// </summary>
        /// <param name="key">The hotkey to unregister.</param>
        public void UnregisterHotkey(Keys key)
        {
            this.UnregisterHotkey(key, HotkeyModifiersKeys.None, HotkeyModifiersKeys.None);
        }

        /// <summary>
        /// Unregister a hotkey.
        /// </summary>
        /// <param name="key">The hotkey to unregister.</param>
        /// <param name="modifier">The modfier of the hotkey.</param>
        public void UnregisterHotkey(Keys key, HotkeyModifiersKeys modifier)
        {
            this.UnregisterHotkey(key, modifier, HotkeyModifiersKeys.None);
        }

        /// <summary>
        /// Unregister a hotkey.
        /// </summary>
        /// <param name="key">The hotkey to unregister.</param>
        /// <param name="modifier">The modfier of the hotkey.</param>
        /// <param name="modifier2">The modfier of the hotkey.</param>
        public void UnregisterHotkey(Keys key, HotkeyModifiersKeys modifier, HotkeyModifiersKeys modifier2)
        {
            var tempList = new Dictionary<int, HotkeyInfo>(registeredHotkeys);

            foreach (var entry in tempList)
            {
                if (entry.Value.Key == key && entry.Value.ModifierKey == modifier && entry.Value.ModifierKey2 == modifier2)
                {
                    NativeMethods.UnregisterHotKey(this.windowHandle.Handle, entry.Key);
                    registeredHotkeys.Remove(entry.Key);
                }
            }
        }

        /// <summary>
        /// Unregisters all the hotkeys.
        /// </summary>
        public void UnregisterAllHotkeys()
        {
            var tempList = new Dictionary<int, HotkeyInfo>(registeredHotkeys);

            foreach (var entry in tempList)
            {
                NativeMethods.UnregisterHotKey(this.windowHandle.Handle, entry.Key);
                registeredHotkeys.Remove(entry.Key);
            }
        }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                HotkeyPressed = null;
                UnregisterAllHotkeys();
                this.windowHandle.Dispose();
            }
        }
    }
}
