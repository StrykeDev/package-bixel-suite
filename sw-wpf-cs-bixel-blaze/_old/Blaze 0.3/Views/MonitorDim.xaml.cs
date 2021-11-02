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


        public MonitorDim()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Dim_Opacity = dimOpacity;
            Properties.Settings.Default.Dim_Speed = dimSpeed;
            Properties.Settings.Default.Dim_Delay = dimDelay;
            Properties.Settings.Default.Save();
        }
    }
}
