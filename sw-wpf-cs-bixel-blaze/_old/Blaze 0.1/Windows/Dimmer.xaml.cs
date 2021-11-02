using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blaze
{
    public partial class Dimmer : Window
    {
        public Dimmer()
        {
            InitializeComponent();
        }

        const int WS_EX_TOPMOST = 0x00000008;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT | WS_EX_TOPMOST);

        }

        public void Fade(bool fadeIn)
        {
            double time = 1;

            if (fadeIn)
            {
                var anim = new DoubleAnimation(Properties.Settings.Default.Dim_Opacity, TimeSpan.FromSeconds(time));
                BeginAnimation(OpacityProperty, anim);
                Console.WriteLine("fading in");
            }
            else
            {
                var anim2 = new DoubleAnimation(0, TimeSpan.FromSeconds(time));
                BeginAnimation(OpacityProperty, anim2);
                Console.WriteLine("fading out");
            }
        }
    }
}
