using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Blaze.ViewModels
{
    class MonitorDim : ObservableObject
    {
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
                    OnPropertyChanged("DimOpacityPercent");
                }
            }
        }
        public string DimOpacityPercent { get { return string.Format("{0}%", (int)(DimOpacity * 100)); } }

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
                    OnPropertyChanged("DimSpeedMS");
                }
            }
        }
        public string DimSpeedMS
        { get { return string.Format("{0} ms", (int)(DimSpeed * 1000)); } }

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
                    OnPropertyChanged("DimDelayMS");
                }
            }
        }
        public string DimDelayMS
        { get { return string.Format("{0} ms", (int)(DimDelay * 1000)); } }

        private bool _listMode = Properties.Settings.Default.BlacklistMode;
        public bool ListMode
        {
            get { return _listMode; }
            set
            {
                if (_listMode != value)
                {
                    _listMode = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ListModeLable");
                }
            }
        }
        public string ListModeLable { get { return ListMode ? "Blacklist" : "Whitelist"; } }

        public ObservableCollection<string> AppList { get; set; } = new ObservableCollection<string>();

        private int _selected = 0;
        public int Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }

        private string _appName = "";
        public string AppName
        {
            get => _appName;
            set
            {
                _appName = value.Replace(".exe","");
                OnPropertyChanged();

            }
        }


        // Entry point
        public MonitorDim()
        {
            FillList();
        }


        private void FillList()
        {
            AppList.Clear();

            foreach (string item in Properties.Settings.Default.Blacklist)
            {
                AppList.Add(item);
            }
        }


        public ICommand Mode => new DelegateCommand(_Mode);
        private void _Mode()
        {
            ListMode = !ListMode;
        }

        public ICommand Add => new DelegateCommand(_Add);
        private void _Add()
        {
            if (!AppList.Contains(AppName))
            {
                AppList.Add(AppName);
            }
            AppName = "";
        }

        public ICommand Remove => new DelegateCommand(_Remove);
        private void _Remove()
        {
            try
            {
                AppList.RemoveAt(Selected);
            }
            catch (Exception)
            {

            }
        }

        public ICommand Undo => new DelegateCommand(_Undo);
        private void _Undo()
        {
            DimOpacity = Properties.Settings.Default.Dim_Opacity;
            DimSpeed = Properties.Settings.Default.Dim_Speed;
            DimDelay = Properties.Settings.Default.Dim_Delay;
            ListMode = Properties.Settings.Default.BlacklistMode;
            FillList();
        }

        public ICommand Apply => new DelegateCommand(_Apply);
        private void _Apply()
        {
            Properties.Settings.Default.Dim_Opacity = DimOpacity;
            Properties.Settings.Default.Dim_Speed = DimSpeed;
            Properties.Settings.Default.Dim_Delay = DimDelay;
            Properties.Settings.Default.BlacklistMode = ListMode;

            if (!Properties.Settings.Default.Blacklist.Equals(AppList))
            {
                Properties.Settings.Default.Blacklist.Clear();

                foreach (string item in AppList)
                {
                    Properties.Settings.Default.Blacklist.Add(item);
                }
            }

            Properties.Settings.Default.Save();
        }
    }
}
