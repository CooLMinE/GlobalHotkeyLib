using GlobalHotkeyLib;
using System;
using System.Windows.Forms;

namespace Test_Project
{
    public partial class Form1 : Form
    {
        GlobalHotkeyHandler gh = new GlobalHotkeyHandler();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gh.RegisterHotkey(Keys.A);
            gh.RegisterHotkey(Keys.A, GlobalHotkeyLib.HotkeyModifiersKeys.Shift);
            gh.RegisterHotkey(Keys.A, GlobalHotkeyLib.HotkeyModifiersKeys.Shift, HotkeyModifiersKeys.Control);
            gh.HotkeyPressed+=gh_HotkeyPressed;
                
        }

        void gh_HotkeyPressed(object sender, GlobalHotkeyLib.HotkeyPressedEventArgs e)
        {
            Console.WriteLine(e.Id);
            Console.WriteLine(e.HotKeyInfo.Key);
            Console.WriteLine(e.HotKeyInfo.ModifierKey);
            Console.WriteLine(e.HotKeyInfo.ModifierKey2);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gh.Dispose();
        }

    }
}
