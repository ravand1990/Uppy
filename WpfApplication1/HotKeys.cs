using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using MessageBox = System.Windows.MessageBox;

namespace Hotkeys
{
    internal class GlobalHotKeys
    {
        private readonly int id;
        private readonly int key;
        private readonly int modifier;
        private IntPtr hWnd;

        public GlobalHotKeys(int modifier, Keys key, IntPtr handle)
        {
            hWnd = handle;
            this.modifier = modifier;
            this.key = (int)key;
            id = GetHashCode();
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }


        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        public bool Unregister()
        {
            return UnregisterHotKey(hWnd, id);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public void testReg(IntPtr handle)
        {
            RegisterHotKey(handle, GetType().GetHashCode(), Constants.Constants.NOMOD, 0x79);
        }

    }

    namespace Constants
    {
        public static class Constants
        {
            //modifiers

            public const int NOMOD = 0x0000;

            public const int ALT = 0x0001;

            public const int CTRL = 0x0002;

            public const int CONTROL = 0x0002;

            public const int SHIFT = 0x0004;

            public const int WIN = 0x0008;


            //windows message id for hotkey

            public const int WM_HOTKEY_MSG_ID = 0x0312;

            public static String getConstant(int mod)
            {
                String mod_ = "";
                switch (mod)
                {
                    case ALT:
                        mod_ = "ALT";
                        break;
                    case CTRL:
                        mod_ = "CTRL";
                        break;
                    case SHIFT:
                        mod_ = "SHIFT";
                        break;
                    case WIN:
                        mod_ = "WIN";
                        break;
                }
                return mod_;
            }

            public static int getConstantInt(String mod)
            {
                mod = mod.ToUpper();
                int mod_ = 0;
                switch (mod)
                {
                    case "ALT":
                        mod_ = ALT;
                        break;
                    case "CTRL":
                        mod_ = CTRL;
                        break;
                    case "CONTROL":
                        mod_ = CTRL;
                        break;
                    case "SHIFT":
                        mod_ = SHIFT;
                        break;
                    case "WIN":
                        mod_ = WIN;
                        break;
                    case "NONE":
                        mod_ = WIN;
                        break;
                }
                return mod_;
            }
        }
    }
}