using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Blaze
{
    public partial class Dimmer : Window
    {
        public Dimmer()
        {
            InitializeComponent();
            Show();
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


        public void FadeIn()
        {
            var anim = new DoubleAnimation(Properties.Settings.Default.Dim_Opacity, TimeSpan.FromSeconds(Properties.Settings.Default.Dim_Speed));
            BeginAnimation(OpacityProperty, anim);
        }

        public void FadeOut()
        {
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(Properties.Settings.Default.Dim_Speed));
            anim.Completed += Anim_Completed;
            BeginAnimation(OpacityProperty, anim);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            Close();
        }
    }
}
