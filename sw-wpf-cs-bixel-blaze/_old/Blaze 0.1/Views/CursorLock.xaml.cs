using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Blaze.Views
{
    public partial class CursorLock : System.Windows.Controls.UserControl
    {
        public System.Windows.Forms.Screen[] arrDisplays = System.Windows.Forms.Screen.AllScreens;
        public int displaysHeight = 0;
        public int displaysWidth = 0;
        public double mostTop = 0;
        public double mostLeft = 0;
        private bool tbFlag = true;


        public static readonly RoutedEvent UpdatedEvent = EventManager.RegisterRoutedEvent(
        "Updated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CursorLock));

        public event RoutedEventHandler Updated
        {
            add { AddHandler(UpdatedEvent, value); }
            remove { RemoveHandler(UpdatedEvent, value); }
        }

        void RaiseUpdatedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CursorLock.UpdatedEvent);
            RaiseEvent(newEventArgs);
        }


        public CursorLock()
        {
            InitializeComponent();

            BtnDetect_Click(this, null);
            BtnUndo_Click(this, null);
        }

        public void DetectDisplays()
        {
            int top = 0;
            int bottom = 0;
            int right = 0;
            int left = 0;

            arrDisplays = System.Windows.Forms.Screen.AllScreens;

            foreach (var display in arrDisplays)
            {
                top = Math.Min(top, display.Bounds.Top);
                bottom = Math.Max(bottom, display.Bounds.Bottom);
                right = Math.Max(right, display.Bounds.Right);
                left = Math.Min(left, display.Bounds.Left);
            }

            displaysHeight = Math.Abs(top) + bottom;
            displaysWidth = Math.Abs(left) + right;
        }


        private void BtnDetect_Click(object sender, RoutedEventArgs e)
        {
            tbFlag = true;

            DetectDisplays();
            mostTop = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Top;
            mostLeft = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Left;

            canvPreviewCanvas.Height = displaysHeight * 2;
            canvPreviewCanvas.Width = displaysWidth * 2;

            canvPreviewCanvas.Children.Clear();

            foreach (var Display in arrDisplays)
            {
                Label lblDisplayPreview = new Label()
                {
                    Content = Display.Bounds.Width + " x " + Display.Bounds.Height,
                    FontSize = Display.Bounds.Width / 10,
                    Height = Display.Bounds.Height,
                    Width = Display.Bounds.Width,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(127, 127, 127)),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Opacity = 0.5,
                    BorderThickness = new Thickness(50),
                    Style = null
                };

                mostTop = Math.Min(mostTop, Display.Bounds.Top);
                mostLeft = Math.Min(mostLeft, Display.Bounds.Left);

                Canvas.SetTop(lblDisplayPreview, Display.Bounds.Top + displaysHeight);
                Canvas.SetLeft(lblDisplayPreview, Display.Bounds.Left + displaysWidth);

                canvPreviewCanvas.Children.Add(lblDisplayPreview);
            }

            canvPreviewCanvas.Children.Add(ccFreeZonePreview);
            BtnUndo_Click(this, null);

            tbFlag = false;
        }

        private void FreeZonePreview_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!tbFlag)
            {
                TextBox tb = sender as TextBox;

                if (int.TryParse(tb.Text, out int temp))
                {
                    ccFreeZonePreview.Height = int.Parse(tbHeight.Text);
                    ccFreeZonePreview.Width = int.Parse(tbWidth.Text);
                    Canvas.SetTop(ccFreeZonePreview, int.Parse(tbY.Text) + displaysHeight);
                    Canvas.SetLeft(ccFreeZonePreview, int.Parse(tbX.Text) + displaysWidth);
                }
                else
                {
                    tb.Text = "0";
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tbFlag = true;

            ccFreeZonePreview.Height = displaysHeight;
            ccFreeZonePreview.Width = displaysWidth;
            Canvas.SetTop(ccFreeZonePreview, mostTop + displaysHeight);
            Canvas.SetLeft(ccFreeZonePreview, mostLeft + displaysWidth);

            tbHeight.Text = ccFreeZonePreview.Height.ToString();
            tbWidth.Text = ccFreeZonePreview.Width.ToString();
            tbX.Text = (Canvas.GetLeft(ccFreeZonePreview) - displaysWidth).ToString();
            tbY.Text = (Canvas.GetTop(ccFreeZonePreview) - displaysHeight).ToString();

            tbFlag = false;
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            tbFlag = true;

            tbHeight.Text = Properties.Settings.Default.CursorLock_Height.ToString();
            tbWidth.Text = Properties.Settings.Default.CursorLock_Width.ToString();
            tbX.Text = Properties.Settings.Default.CursorLock_X.ToString();
            tbY.Text = Properties.Settings.Default.CursorLock_Y.ToString();

            ccFreeZonePreview.Height = Properties.Settings.Default.CursorLock_Height;
            ccFreeZonePreview.Width = Properties.Settings.Default.CursorLock_Width;
            Canvas.SetTop(ccFreeZonePreview, Properties.Settings.Default.CursorLock_Y + displaysHeight);
            Canvas.SetLeft(ccFreeZonePreview, Properties.Settings.Default.CursorLock_X + displaysWidth);

            tbFlag = false;
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CursorLock_Height = Convert.ToInt32(ccFreeZonePreview.Height);
            Properties.Settings.Default.CursorLock_Width = Convert.ToInt32(ccFreeZonePreview.Width);
            Properties.Settings.Default.CursorLock_X = Convert.ToInt32(Canvas.GetTop(ccFreeZonePreview) - displaysHeight);
            Properties.Settings.Default.CursorLock_Y = Convert.ToInt32(Canvas.GetLeft(ccFreeZonePreview) - displaysWidth);
            Properties.Settings.Default.Save();

            RaiseUpdatedEvent();
        }
    }
}
