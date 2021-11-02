using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Windows.Media.Animation;
using System.Collections.Specialized;


namespace Blaze.Views
{
    public partial class MonitorDim : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Settings
        private double _dimOpacity = Properties.Settings.Default.Dim_Opacity;
        public double DimOpacity
        {
            get { return _dimOpacity; }
            set
            {
                if (_dimOpacity != value)
                {
                    _dimOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _dimSpeed = Properties.Settings.Default.Dim_Speed;
        public double DimSpeed
        {
            get { return _dimSpeed; }
            set
            {
                if (_dimSpeed != value)
                {
                    _dimSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _dimDelay = Properties.Settings.Default.Dim_Delay;
        public double DimDelay
        {
            get { return _dimDelay; }
            set
            {
                if (_dimDelay != value)
                {
                    _dimDelay = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _blacklistMode = Properties.Settings.Default.BlacklistMode;
        public bool BlacklistMode
        {
            get { return _blacklistMode; }
            set
            {
                if (_blacklistMode != value)
                {
                    _blacklistMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private StringCollection blacklistedApps = Properties.Settings.Default.Blacklist;


        public MonitorDim()
        {
            DataContext = this;
            InitializeComponent();
            lbBlacklist.ItemsSource = blacklistedApps;
            lbBlacklist.Items.Refresh();
        }


        private void BtnMode_Click(object sender, RoutedEventArgs e)
        {
            if (BlacklistMode)
            {
                BlacklistMode = false;
            }
            else
            {
                BlacklistMode = true;
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbBlacklist.SelectedIndex >= 0)
            {
                blacklistedApps.RemoveAt(lbBlacklist.SelectedIndex);
            }
            lbBlacklist.Items.Refresh();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbxAppName.Text.Length > 0)
            {
                if (!blacklistedApps.Contains(tbxAppName.Text.ToLower()))
                {
                    blacklistedApps.Add(tbxAppName.Text.ToLower());
                }
                tbxAppName.Text = null;
                lbBlacklist.Items.Refresh();
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Dim_Opacity = _dimOpacity;
            Properties.Settings.Default.Dim_Speed = _dimSpeed;
            Properties.Settings.Default.Dim_Delay = _dimDelay;
            Properties.Settings.Default.Blacklist = blacklistedApps;
            Properties.Settings.Default.BlacklistMode = _blacklistMode;
            Properties.Settings.Default.Save();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            DimOpacity = Properties.Settings.Default.Dim_Opacity;
            DimSpeed = Properties.Settings.Default.Dim_Speed;
            DimDelay = Properties.Settings.Default.Dim_Delay;
            BlacklistMode = Properties.Settings.Default.BlacklistMode;
            blacklistedApps = Properties.Settings.Default.Blacklist;
            lbBlacklist.Items.Refresh();
        }

        private bool _preview = true;
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            grdOptions.IsEnabled = false;
            lblPreview.Content = "Preview in progress...";

            if (_preview)
            {
                var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(_dimSpeed));
                anim.Completed += Anim_Completed;
                recDimmerPreview.BeginAnimation(OpacityProperty, anim);
                _preview = false;
            }
            else
            {
                var anim = new DoubleAnimation(_dimOpacity, TimeSpan.FromSeconds(_dimSpeed));
                anim.FillBehavior = FillBehavior.Stop;
                anim.BeginTime = TimeSpan.FromSeconds(_dimDelay);
                anim.Completed += Anim_Completed;
                recDimmerPreview.BeginAnimation(OpacityProperty, anim);
                _preview = true;
            }
        }
        private void Anim_Completed(object sender, EventArgs e)
        {
            grdOptions.IsEnabled = true;
            lblPreview.Content = "Click on the image to preview";
        }
    }
}
