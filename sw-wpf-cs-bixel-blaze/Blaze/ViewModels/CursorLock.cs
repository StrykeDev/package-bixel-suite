using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Screen = System.Windows.Forms.Screen;
using Blaze.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blaze.ViewModels
{
    class CursorLock : ObservableObject
    {
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
                    OnPropertyChanged("PreviewLeft");
                }
            }
        }
        public int PreviewLeft
        {
            get { return _previewX + _displaysWidth; }
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
                    OnPropertyChanged("PreviewTop");
                }
            }
        }
        public int PreviewTop
        {
            get { return _previewY + _displaysHeight; }
        }

        private int _canvasHeight = 0;
        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set
            {
                if (_canvasHeight != value)
                {
                    _canvasHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _canvasWidth = 0;
        public int CanvasWidth
        {
            get { return _canvasWidth; }
            set
            {
                if (_canvasWidth != value)
                {
                    _canvasWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PreviewDisplay> Displays { get; set; } = new ObservableCollection<PreviewDisplay> { };

        
        /// <summary>
        /// CursorLock ViewModel.
        /// </summary>
        public CursorLock()
        {
            _Detect();
        }


        private int _displaysHeight = 0;
        private int _displaysWidth = 0;
        private int _top = 0;
        private int _bottom = 0;
        private int _right = 0;
        private int _left = 0;
        public ICommand Detect => new DelegateCommand(_Detect);
        private void _Detect()
        {
            Screen[] screens = Screen.AllScreens;

            // Get desktop bounds and location
            foreach (var screen in screens)
            {
                _top = Math.Min(_top, screen.Bounds.Top);
                _bottom = Math.Max(_bottom, screen.Bounds.Bottom);
                _right = Math.Max(_right, screen.Bounds.Right);
                _left = Math.Min(_left, screen.Bounds.Left);
            }

            _displaysHeight = Math.Abs(_top) + _bottom;
            _displaysWidth = Math.Abs(_left) + _right;


            // Setup the canvas
            CanvasHeight = _displaysHeight * 2;
            CanvasWidth = _displaysWidth * 2;


            // Setup the displays
            Displays.Clear();
            foreach (var screen in screens)
            {
                Displays.Add(new PreviewDisplay(
                    screen.Bounds.Height,
                    screen.Bounds.Width,
                    screen.Bounds.Top + _displaysHeight,
                    screen.Bounds.Left + _displaysWidth));
            }
        }

        public ICommand Reset => new DelegateCommand(_Reset);
        private void _Reset()
        {
            _Detect();
            PreviewHeight = _displaysHeight;
            PreviewWidth = _displaysWidth;
            PreviewX = _left;
            PreviewY = _top;
        }

        public ICommand Undo => new DelegateCommand(_Undo);
        private void _Undo()
        {
            _Detect();
            PreviewHeight = Properties.Settings.Default.CursorLock_Height;
            PreviewWidth = Properties.Settings.Default.CursorLock_Width;
            PreviewX = Properties.Settings.Default.CursorLock_X;
            PreviewY = Properties.Settings.Default.CursorLock_Y;
        }

        public ICommand Apply => new DelegateCommand(_Apply);
        private void _Apply()
        {
            Properties.Settings.Default.CursorLock_Height = _previewHeight;
            Properties.Settings.Default.CursorLock_Width = _previewWidth;
            Properties.Settings.Default.CursorLock_X = _previewX;
            Properties.Settings.Default.CursorLock_Y = _previewY;
            Properties.Settings.Default.Save();
        }
    }
}
