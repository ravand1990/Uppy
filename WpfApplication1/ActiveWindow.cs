using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Shapes;

namespace FileUpload
{
    class ActiveWindow
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(int hWnd, ref System.Drawing.Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowInfo(int hWnd, ref WINDOWINFO pwi);

        private IntPtr hWnd;


        public ActiveWindow()
        {
            this.hWnd = GetForegroundWindow();
        }

        public WINDOWINFO GetWindowinfo()
        {
            WINDOWINFO info = new WINDOWINFO();

            GetWindowInfo(hWnd.ToInt32(), ref info);


            return info;
        }

        public struct RECT
        {
            public int Left;       // x-coordinate of the upper-left 
            public int Top;        // y-coordinate of the upper-left 

            public int Right;      // x-coordinate of the lower-right 
            public int Bottom;     // y-coordinate of the lower-right

        }

        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;
        }
    }
}
