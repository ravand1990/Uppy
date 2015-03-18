using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace FileUpload
{
    class ScreenShotHandler
    {
        private System.Drawing.Point p1;
        private System.Drawing.Point p2;

        private ActiveWindow activeWindow;

        public ScreenShotHandler()
        { }


        public ScreenShotHandler(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public Bitmap GetSelectionScreenShot()
        {
            Thread.SpinWait(5000);
            int width = p2.X - p1.X;
            int height = p2.Y - p1.Y;

            Rectangle rect = new Rectangle(p1, new Size(width, height));

            Bitmap img = new Bitmap(Math.Abs(width), Math.Abs(height));
            Graphics graphic = Graphics.FromImage(img);

            Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);


            graphic.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
            return img;
        }

        public Bitmap GetCurrentWindowScreenShot()
        {
            activeWindow = new ActiveWindow();

            ActiveWindow.WINDOWINFO info = activeWindow.GetWindowinfo();

            ActiveWindow.RECT rect = info.rcWindow;

            int width = info.rcWindow.Right - info.rcWindow.Left;
            int height = info.rcWindow.Bottom - info.rcWindow.Top;


            var img = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(img);
            Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);


            graphic.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);

            return img;
        }

        public Bitmap GetFullScreenShot()
        {
            var img = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height,
                PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(img);


            graphic.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0,
                Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            return img;
        }

    }
}
