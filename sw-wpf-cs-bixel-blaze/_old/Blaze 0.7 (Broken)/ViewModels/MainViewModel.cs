using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using Screen = System.Windows.Forms.Screen;
using Blaze.Models;

namespace Blaze.ViewModels
{
    class MainViewModel : ObservableObject
    {
        // Metadata
        public static string _donateUrl = @"https://www.bixel.io/donate?amount=1";
        public static string _websiteUrl = @"https://www.bixel.io";
        public static string BuildName { get; } = GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration);
        public static string Copyright { get; } = GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);

        private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }


        // NotifyIcon
        public static MyNotifyIcon _notify = new MyNotifyIcon();


        // Services flags
        private static bool _pauseCL = false;
        private static bool _pauseMD = false;


        // Threads
        private static Thread thHotkeys = new Thread(HotkeysService);
        private static Thread thActiveApp = new Thread(ActiveAppsService);
        private static Thread thCursorLock = new Thread(CursorLockService);
        private static Thread thMonitorDim = new Thread(MonitorDimService);


        // Settings
        private static RegistryKey regKeyStartup = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private static bool _startup = regKeyStartup.GetValue("Blaze") != null;
        public static bool Startup
        {
            get { return _startup; }
            set
            {
                if (_startup != value)
                {
                    _startup = value;
                    if (value)
                    {
                        regKeyStartup.SetValue("Blaze", '"' + System.Reflection.Assembly.GetExecutingAssembly().Location + '"');
                    }
                    else
                    {
                        regKeyStartup.DeleteValue("Blaze");
                    }
                }
            }
        }

        private static bool _startMinimized = Properties.Settings.Default.Start_Min;
        public static bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                if (_startMinimized != value)
                {
                    _startMinimized = value;
                    Properties.Settings.Default.Start_Min = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private static int _updateInterval = Properties.Settings.Default.Update_Interval;
        public static int UpdateInterval
        {
            get { return _updateInterval; }
            set
            {
                if (_updateInterval != value)
                {
                    _updateInterval = value;
                    Properties.Settings.Default.Update_Interval = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private static bool _enableCursorLock = false;
        public static bool EnableCursorLock
        {
            get { return _enableCursorLock; }
            set
            {
                if (_enableCursorLock != value)
                {
                    _enableCursorLock = value;
                    Properties.Settings.Default.Enable_CursorLock = value;
                    Properties.Settings.Default.Save();

                    if (value)
                    {
                        if (!thCursorLock.IsAlive)
                        {
                            thCursorLock = new Thread(CursorLockService);
                            thCursorLock.SetApartmentState(ApartmentState.STA);

                            thCursorLock.Start();
                        }
                    }
                    else
                    {
                        if (thCursorLock.IsAlive)
                        {
                            thCursorLock.Join();
                        }
                    }
                }
            }
        }

        private static bool _enableMonitorDim = false;
        public static bool EnableMonitorDim
        {
            get { return _enableMonitorDim; }
            set
            {
                if (_enableMonitorDim != value)
                {
                    _enableMonitorDim = value;
                    Properties.Settings.Default.Enable_MonitorDim = value;
                    Properties.Settings.Default.Save();

                    if (value)
                    {
                        if (!thMonitorDim.IsAlive)
                        {
                            thMonitorDim = new Thread(MonitorDimService);
                            thMonitorDim.SetApartmentState(ApartmentState.STA);
                            thMonitorDim.Start();
                        }
                    }
                    else
                    {
                        if (thMonitorDim.IsAlive)
                        {
                            thMonitorDim.Join();
                        }
                    }
                }
            }
        }


        public MainViewModel()
        {
            if (Properties.Settings.Default.Blacklist == null)
            {
                Properties.Settings.Default.Blacklist = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }

            StartBackgroundServices();
            EnableCursorLock = Properties.Settings.Default.Enable_CursorLock;
            EnableMonitorDim = Properties.Settings.Default.Enable_MonitorDim;
        }


        #region Background services 

        private void StartBackgroundServices()
        {
            thHotkeys.SetApartmentState(ApartmentState.STA);
            thHotkeys.IsBackground = true;
            if (!thHotkeys.IsAlive)
            {
                thHotkeys.Start();
            }
            
            thActiveApp.SetApartmentState(ApartmentState.STA);
            thActiveApp.IsBackground = true;
            if (!thActiveApp.IsAlive)
            {
                thActiveApp.Start();
            }
        }

        // Hotkeys service
        private static void HotkeysService()
        {
            while (true)
            {
                if (Keyboard.IsKeyToggled(Key.Scroll))
                {
                    _pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    _pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    _pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    _pauseCL = true;
                }
                else
                {
                    _pauseCL = false;
                }

                Thread.Sleep(_updateInterval);
            }
        }


        // Fullscreen service
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static bool _fullscreenApp = false;
        public static int _fullscreenAppId = 0;
        public static Screen _activeDisplay;

        private static void ActiveAppsService()
        {
            RECT rect = new RECT();
            IntPtr hWnd;

            while (true)
            {
                hWnd = GetForegroundWindow();
                GetWindowRect(new HandleRef(null, hWnd), ref rect);
                _activeDisplay = Screen.FromHandle(hWnd);

                if (_activeDisplay.Bounds.Width == (rect.right - rect.left) && _activeDisplay.Bounds.Height == (rect.bottom - rect.top))
                {
                    GetWindowThreadProcessId(hWnd, out int pid);
                    if (pid != _fullscreenAppId)
                    {
                        _fullscreenAppId = pid;
                        _fullscreenApp = true;
                    }
                }
                else
                {
                    if (_fullscreenApp)
                    {
                        _fullscreenAppId = 0;
                        _fullscreenApp = false;
                    }
                }

                Thread.Sleep(_updateInterval);
            }
        }

        #endregion

        #region Cursor Lock service

        private static void CursorLockService()
        {
            Rectangle clipRec = new Rectangle();
            bool flag;

            while (_enableCursorLock)
            {
                if ((_pauseCL || _fullscreenApp) && _enableCursorLock)
                {
                    System.Windows.Forms.Cursor.Clip = new Rectangle();
                    flag = true;
                    while (flag && _enableCursorLock)
                    {
                        System.Drawing.Point pos = System.Windows.Forms.Cursor.Position;
                        if (pos.X > clipRec.Top && pos.X < clipRec.Width && pos.Y > clipRec.Y && pos.Y < clipRec.Height)
                        {
                            flag = false;
                        }

                        Thread.Sleep(_updateInterval);
                    }
                }
                else
                {
                    clipRec.Height = Properties.Settings.Default.CursorLock_Height;
                    clipRec.Width = Properties.Settings.Default.CursorLock_Width;
                    clipRec.X = Properties.Settings.Default.CursorLock_X;
                    clipRec.Y = Properties.Settings.Default.CursorLock_Y;

                    System.Windows.Forms.Cursor.Clip = clipRec;
                }

                Thread.Sleep(_updateInterval);
            }

            System.Windows.Forms.Cursor.Clip = new Rectangle();
        }

        #endregion

        #region Monitor Dim service

        private static void MonitorDimService()
        {
            var displays = Screen.AllScreens;
            bool dimFlag = false;
            int pId = 0;
            string pName = "";

            while (_enableMonitorDim)
            {
                if (_fullscreenApp)
                {
                    // Delayed start
                    Thread.Sleep((int)(Properties.Settings.Default.Dim_Delay * 1000));
                    if (_fullscreenApp && pId == _fullscreenAppId)
                    {
                        // Reset dimmers if display setup changed
                        if (!displays.Equals(Screen.AllScreens))
                        {
                            displays = Screen.AllScreens;
                            Application.Current.Dispatcher.Invoke(() => DestroyDimmers());
                            dimFlag = false;
                        }

                        // Spawn dimmers if not already spawned
                        if (!dimFlag)
                        {
                            var blacklist = Properties.Settings.Default.Blacklist;
                            try
                            {
                                pName = Process.GetProcessById(_fullscreenAppId).ProcessName.ToLower();
                            }
                            catch (Exception)
                            {
                                pName = "";
                            }

                            if (Properties.Settings.Default.BlacklistMode)  // Blacklist mode
                            {
                                if (blacklist.Count == 0 || !blacklist.Contains(pName))
                                {
                                    Application.Current.Dispatcher.Invoke(() => CreateDimmers(displays));
                                    dimFlag = true;
                                }
                            }
                            else                                            // Whitelist mode
                            {
                                if (blacklist.Count > 0)
                                {
                                    if (blacklist.Contains(pName))
                                    {
                                        Application.Current.Dispatcher.Invoke(() => CreateDimmers(displays));
                                        dimFlag = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() => DestroyDimmers());
                        pId = _fullscreenAppId;
                        dimFlag = false;
                    }
                }
                else
                {
                    // Destroy dimmers if not already destroyed
                    if (dimFlag)
                    {
                        Application.Current.Dispatcher.Invoke(() => DestroyDimmers());
                        dimFlag = false;
                    }
                }

                Thread.Sleep(_updateInterval);
            }
        }

        private static Dimmer[] _dimmers;

        private static void CreateDimmers(Screen[] displayref)
        {
            _dimmers = new Dimmer[displayref.Length];
            for (int i = 0; i < displayref.Length; i++)
            {
                _dimmers[i] = new Dimmer()
                {
                    Height = displayref[i].Bounds.Height,
                    Width = displayref[i].Bounds.Width,
                    Top = displayref[i].Bounds.Top,
                    Left = displayref[i].Bounds.Left
                };
            }

            for (int i = 0; i < displayref.Length; i++)
            {
                if (!displayref[i].Equals(_activeDisplay))
                {
                    _dimmers[i].FadeIn();
                }
            }
        }

        private static void DestroyDimmers()
        {
            if (_dimmers != null)
            {
                foreach (var dimmer in _dimmers)
                {
                    dimmer.FadeOut();
                }
            }
        }

        #endregion

        #region Controls events

        private void TabNavMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _notify.Notify_Exit(sender, e);
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Hide();
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e) => Process.Start(_donateUrl);

        private void BtnWebsite_Click(object sender, RoutedEventArgs e) => Process.Start(_websiteUrl);

        #endregion
    }
}
