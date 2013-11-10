using System;
using System.Runtime.InteropServices;

namespace GlobalHotkeyLib.Utilities
{
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
}
