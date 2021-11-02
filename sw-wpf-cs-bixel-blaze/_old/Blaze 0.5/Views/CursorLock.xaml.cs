using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Screen = System.Windows.Forms.Screen;


namespace Blaze.Views
{
    public partial class CursorLock : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            DetectDisplays();
        }

        private int previewHeight = Properties.Settings.Default.CursorLock_Height;
        public int PreviewHeight
        {
            get { return previewHeight; }
            set
            {
                if (previewHeight != value)
                {
                    previewHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        private int previewWidth = Properties.Settings.Default.CursorLock_Width;
        public int PreviewWidth
        {
            get { return previewWidth; }
            set
            {
                if (previewWidth != value)
                {
                    previewWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        private int previewX = Properties.Settings.Default.CursorLock_X;
        public int PreviewX
        {
            get { return previewX; }
            set
            {
                if (previewX != value)
                {
                    previewX = value;
                    OnPropertyChanged();
                }
            }
        }

        private int previewY = Properties.Settings.Default.CursorLock_Y;
        public int PreviewY
        {
            get { return previewY; }
            set
            {
                if (previewY != value)
                {
                    previewY = value;
                    OnPropertyChanged();
                }
            }
        }


        // Entry point
        public CursorLock()
        {
            DataContext = this;
            InitializeComponent();
            DetectDisplays();
        }


        private int displaysHeight = 0;
        private int displaysWidth = 0;
        int top = 0;
        int bottom = 0;
        int right = 0;
        int left = 0;
        public void DetectDisplays()
        {

            foreach (var screen in Screen.AllScreens)
            {
                top = Math.Min(top, screen.Bounds.Top);
                bottom = Math.Max(bottom, screen.Bounds.Bottom);
                right = Math.Max(right, screen.Bounds.Right);
                left = Math.Min(left, screen.Bounds.Left);
            }

            displaysHeight = Math.Abs(top) + bottom;
            displaysWidth = Math.Abs(left) + right;

            canvasPreview.Height = displaysHeight * 2;
            canvasPreview.Width = displaysWidth * 2;

            canvasPreview.Children.Clear();
            foreach (var Display in Screen.AllScreens)
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

                Canvas.SetTop(lblDisplayPreview, Display.Bounds.Top + displaysHeight);
                Canvas.SetLeft(lblDisplayPreview, Display.Bounds.Left + displaysWidth);

                canvasPreview.Children.Add(lblDisplayPreview);
            }

            canvasPreview.Children.Add(ccZonePreview);
            ccZonePreview.Height = previewHeight;
            ccZonePreview.Width = previewWidth;
            Canvas.SetTop(ccZonePreview, previewY + displaysHeight);
            Canvas.SetLeft(ccZonePreview, previewX + displaysWidth);
        }


        private void BtnDetect_Click(object sender, RoutedEventArgs e)
        {
            DetectDisplays();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            PreviewHeight = displaysHeight;
            PreviewWidth = displaysWidth;
            PreviewX = left;
            PreviewY = top;
            DetectDisplays();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            PreviewHeight = Properties.Settings.Default.CursorLock_Height;
            PreviewWidth = Properties.Settings.Default.CursorLock_Width;
            PreviewX = Properties.Settings.Default.CursorLock_X;
            PreviewY = Properties.Settings.Default.CursorLock_Y;
            DetectDisplays();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e) // Restart CL serive if on
        {
            Properties.Settings.Default.CursorLock_Height = previewHeight;
            Properties.Settings.Default.CursorLock_Width = previewWidth;
            Properties.Settings.Default.CursorLock_X = previewX;
            Properties.Settings.Default.CursorLock_Y = previewY;
            Properties.Settings.Default.Save();
        }
    }
}
