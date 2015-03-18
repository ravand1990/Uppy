using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FileUpload;
using Brushes = System.Windows.Media.Brushes;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WpfApplication1
{
    /// <summary>
    ///     Interaction logic for DrawForm.xaml
    /// </summary>
    public partial class DrawForm : Window
    {
        private readonly Canvas c;
        private bool mouseDown;
        private Point p1;
        private Point p2;

        private ScreenShotHandler scr;

        public DrawForm()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            Height = Screen.PrimaryScreen.Bounds.Height;
            Width = Screen.PrimaryScreen.Bounds.Width;
            WindowState = WindowState.Maximized;
            Focus();
            Topmost = true;
            c = LogicalTreeHelper.GetChildren(this).OfType<Canvas>().FirstOrDefault();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            p1 = e.GetPosition(this);
            if (!mouseDown)
            {
                mouseDown = true;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            c.Children.Clear();
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            UpdateLayout();
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            InvalidateVisual();
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);


            mouseDown = false;
            c.Children.Clear();
            var p1_ = new System.Drawing.Point((int) p1.X, (int) p1.Y);
            var p2_ = new System.Drawing.Point((int) p2.X, (int) p2.Y);
            scr = new ScreenShotHandler(p1_, p2_);
            Bitmap img = scr.GetSelectionScreenShot();
            img.Save(MemberWindow.SAVEFOLDER + "/test.jpg");
            Close();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            c.Children.Clear();

            if (mouseDown)
            {
                var r = new Rectangle();

                Canvas.SetTop(c, 50);
                Canvas.SetLeft(c, 50);

                p2 = e.GetPosition(this);

                double x = Math.Min(p2.X, p1.X);
                double y = Math.Min(p2.Y, p1.Y);

                double w = Math.Max(p2.X, p1.X) - x;
                double h = Math.Max(p2.Y, p1.Y) - y;


                r.StrokeThickness = 2;
                r.Width = w;
                r.Height = h;
                r.SetValue(Canvas.LeftProperty, Math.Min(p2.X, p1.X));
                r.SetValue(Canvas.TopProperty, Math.Min(p2.Y, p1.Y));
                r.IsHitTestVisible = false;
                r.Stroke = Brushes.Red;
                r.Fill = (SolidColorBrush) (new BrushConverter().ConvertFrom("#50ffaacc"));


                c.Children.Add(r);
            }
            else
            {
                c.Children.Clear();
            }
        }
    }
}