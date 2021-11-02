using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System;
using System.Windows.Media.Animation;
using System.Windows.Data;
using System.Runtime.InteropServices;

namespace Blaze.Views
{
    public partial class MonitorDim : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double dimOpacity = Properties.Settings.Default.Dim_Opacity;
        public double DimOpacity
        {
            get { return dimOpacity; }
            set
            {
                if (dimOpacity != value)
                {
                    dimOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private double dimSpeed = Properties.Settings.Default.Dim_Speed;
        public double DimSpeed
        {
            get { return dimSpeed; }
            set
            {
                if (dimSpeed != value)
                {
                    dimSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        private double dimDelay = Properties.Settings.Default.Dim_Delay;
        public double DimDelay
        {
            get { return dimDelay; }
            set
            {
                if (dimDelay != value)
                {
                    dimDelay = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool blacklistMode = Properties.Settings.Default.BlacklistMode;
        public bool BlacklistMode
        {
            get { return blacklistMode; }
            set
            {
                if (blacklistMode != value)
                {
                    blacklistMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private System.Collections.Specialized.StringCollection blacklistedApps = Properties.Settings.Default.Blacklist;


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
            Properties.Settings.Default.Dim_Opacity = dimOpacity;
            Properties.Settings.Default.Dim_Speed = dimSpeed;
            Properties.Settings.Default.Dim_Delay = dimDelay;
            Properties.Settings.Default.Blacklist = blacklistedApps;
            Properties.Settings.Default.BlacklistMode = blacklistMode;
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

        private bool preview = true;
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            grdOptions.IsEnabled = false;
            lblPreview.Content = "Preview in progress...";

            if (preview)
            {
                var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(dimSpeed));
                anim.Completed += Anim_Completed;
                recDimmerPreview.BeginAnimation(OpacityProperty, anim);
                preview = false;
            }
            else
            {
                var anim = new DoubleAnimation(dimOpacity, TimeSpan.FromSeconds(dimSpeed));
                anim.FillBehavior = FillBehavior.Stop;
                anim.BeginTime = TimeSpan.FromSeconds(dimDelay);
                anim.Completed += Anim_Completed;
                recDimmerPreview.BeginAnimation(OpacityProperty, anim);
                preview = true;
            }
        }
        private void Anim_Completed(object sender, EventArgs e)
        {
            grdOptions.IsEnabled = true;
            lblPreview.Content = "Click on the image to preview";
        }
    }
}
