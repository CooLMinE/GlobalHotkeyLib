using GlobalHotkeyLib.Utilities;
using System;
using System.Windows.Forms;

namespace GlobalHotkeyLib
{
    /// <summary>
    /// This class inherits from NativeWindow in order to create a handle so it can receive the window messages.
    /// </summary>
    internal class HotkeyHandle : NativeWindow, IDisposable
    {
        private volatile bool isDisposing = false;
        private GlobalHotkeyHandler handler;
        public event EventHandler<HotkeyPressedEventArgs> HotkeyMessageReceived;

        /// <summary>
        /// Constructor of HotkeyHandle.
        /// </summary>
        /// <param name="parent">The object that contains all the registered hotkeys.</param>
        public HotkeyHandle(GlobalHotkeyHandler parent)
        {
            this.handler = parent;

            try
            {
                this.CreateHandle(new CreateParams());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WindowMessages.WM_NCDESTROY)
            {
                this.Dispose();
            }

            if (m.Msg == WindowMessages.WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                OnHotkeyMessageReceived(new HotkeyPressedEventArgs(id, handler.GetHotkeyInfoById(id)));
            }
        }

        /// <summary>
        /// A WM_HOTKEY has been received. 
        /// </summary>
        /// <param name="e">A HotkeyPressedEventArgs object containing the information about the event.</param>
        protected virtual void OnHotkeyMessageReceived(HotkeyPressedEventArgs e)
        {
            if (HotkeyMessageReceived != null)
                HotkeyMessageReceived(this, e);
        }

        /// <summary>
        /// Get the handle of the window created by this class.
        /// </summary>
        /// <returns></returns>
        public IntPtr GetHandle()
        {
            return this.Handle;
        }

        /// <summary>
        /// Release the resources.
        /// </summary>
        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                HotkeyMessageReceived = null;
                this.DestroyHandle();
            }
        }

    }

}
