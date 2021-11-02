using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            UpdateBlacklist();
        }

        private void UpdateBlacklist()
        {
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
            UpdateBlacklist();
        }
    }
}
