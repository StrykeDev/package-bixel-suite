using System;
using System.ComponentModel;
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

        private int _previewHeight = Properties.Settings.Default.CursorLock_Height;
        public int PreviewHeight
        {
            get { return _previewHeight; }
            set
            {
                if (_previewHeight != value)
                {
                    _previewHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _previewWidth = Properties.Settings.Default.CursorLock_Width;
        public int PreviewWidth
        {
            get { return _previewWidth; }
            set
            {
                if (_previewWidth != value)
                {
                    _previewWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _previewX = Properties.Settings.Default.CursorLock_X;
        public int PreviewX
        {
            get { return _previewX; }
            set
            {
                if (_previewX != value)
                {
                    _previewX = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _previewY = Properties.Settings.Default.CursorLock_Y;
        public int PreviewY
        {
            get { return _previewY; }
            set
            {
                if (_previewY != value)
                {
                    _previewY = value;
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


        private int _displaysHeight = 0;
        private int _displaysWidth = 0;
        private int _top = 0;
        private int _bottom = 0;
        private int _right = 0;
        private int _left = 0;
        public void DetectDisplays()
        {
            foreach (var screen in Screen.AllScreens)
            {
                _top = Math.Min(_top, screen.Bounds.Top);
                _bottom = Math.Max(_bottom, screen.Bounds.Bottom);
                _right = Math.Max(_right, screen.Bounds.Right);
                _left = Math.Min(_left, screen.Bounds.Left);
            }

            _displaysHeight = Math.Abs(_top) + _bottom;
            _displaysWidth = Math.Abs(_left) + _right;

            canvasPreview.Height = _displaysHeight * 2;
            canvasPreview.Width = _displaysWidth * 2;

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

                Canvas.SetTop(lblDisplayPreview, Display.Bounds.Top + _displaysHeight);
                Canvas.SetLeft(lblDisplayPreview, Display.Bounds.Left + _displaysWidth);

                canvasPreview.Children.Add(lblDisplayPreview);
            }

            canvasPreview.Children.Add(ccZonePreview);
            ccZonePreview.Height = _previewHeight;
            ccZonePreview.Width = _previewWidth;
            Canvas.SetTop(ccZonePreview, _previewY + _displaysHeight);
            Canvas.SetLeft(ccZonePreview, _previewX + _displaysWidth);
        }


        private void BtnDetect_Click(object sender, RoutedEventArgs e)
        {
            DetectDisplays();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            PreviewHeight = _displaysHeight;
            PreviewWidth = _displaysWidth;
            PreviewX = _left;
            PreviewY = _top;
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

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CursorLock_Height = _previewHeight;
            Properties.Settings.Default.CursorLock_Width = _previewWidth;
            Properties.Settings.Default.CursorLock_X = _previewX;
            Properties.Settings.Default.CursorLock_Y = _previewY;
            Properties.Settings.Default.Save();
        }
    }
}
