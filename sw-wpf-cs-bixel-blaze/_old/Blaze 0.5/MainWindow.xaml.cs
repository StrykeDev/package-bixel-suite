using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Screen = System.Windows.Forms.Screen;
using System.Reflection;

namespace Blaze
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metadata
        public string donateURL = @"https://www.bixel.io/donate?amount=1";
        public string websiteURL = @"https://www.bixel.io";
        private static string versionName = GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration);
        public static string VersionName
        {
            get { return versionName; }
        }

        private static string copyright = GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);
        public static string Copyright
        {
            get { return copyright; }
        }

        private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }

        // Services flags
        public static bool pauseCL = false;
        public static bool pauseMD = false;

        // Threads
        public Thread thHotkeys = new Thread(HotkeysService);
        public Thread thActiveApp = new Thread(ActiveAppsService);
        public Thread thCursorLock = new Thread(CursorLockService);
        public Thread thMonitorDim = new Thread(MonitorDimService);

        // Loading settings
        public static RegistryKey regKeyStartup = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private bool startup = regKeyStartup.GetValue("Blaze") != null;
        public bool Startup
        {
            get { return startup; }
            set
            {
                if (startup != value)
                {
                    startup = value;
                    if (value)
                    {
                        regKeyStartup.SetValue("Blaze", '"' + System.Reflection.Assembly.GetExecutingAssembly().Location + '"');
                    }
                    else
                    {
                        regKeyStartup.DeleteValue("Blaze");
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool startMinimized = Properties.Settings.Default.Start_Min;
        public bool StartMinimized
        {
            get { return startMinimized; }
            set
            {
                if (startMinimized != value)
                {
                    startMinimized = value;
                    Properties.Settings.Default.Start_Min = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        private static int updateInterval = Properties.Settings.Default.Update_Interval;

        private static bool enableCursorLock = false;
        public bool EnableCursorLock
        {
            get { return enableCursorLock; }
            set
            {
                if (enableCursorLock != value)
                {
                    enableCursorLock = value;
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

                    OnPropertyChanged();
                }
            }
        }

        private static bool enableMonitorDim = false;
        public bool EnableMonitorDim
        {
            get { return enableMonitorDim; }
            set
            {
                if (enableMonitorDim != value)
                {
                    enableMonitorDim = value;
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

                    OnPropertyChanged();
                }
            }
        }



        // Entry point
        public MainWindow()
        {
            if (Properties.Settings.Default.Blacklist == null)
            {
                Properties.Settings.Default.Blacklist = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }
            DataContext = this;
            InitializeComponent();

            StartBackgroundServices();
            EnableCursorLock = Properties.Settings.Default.Enable_CursorLock;
            EnableMonitorDim = Properties.Settings.Default.Enable_MonitorDim;
        }


        #region Background services 
        private void StartBackgroundServices()
        {
            thHotkeys.SetApartmentState(ApartmentState.STA);
            thHotkeys.IsBackground = true;
            thHotkeys.Start();

            thActiveApp.SetApartmentState(ApartmentState.STA);
            thActiveApp.IsBackground = true;
            thActiveApp.Start();
        }

        // Hotkeys service
        private static void HotkeysService()
        {
            while (true)
            {
                if (Keyboard.IsKeyToggled(Key.Scroll))
                {
                    pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    pauseCL = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    pauseCL = true;
                }
                else
                {
                    pauseCL = false;
                }

                Thread.Sleep(updateInterval);
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

        //Used to get ID of any Window
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        public static bool fullscreenApp = false;
        public static string fullscreenAppName = "";
        public static Screen activeDisplay;

        private static void ActiveAppsService()
        {
            RECT rect = new RECT();
            IntPtr hWnd;

            while (true)
            {
                hWnd = GetForegroundWindow();
                GetWindowRect(new HandleRef(null, hWnd), ref rect);
                activeDisplay = Screen.FromHandle(hWnd);

                if (activeDisplay.Bounds.Width == (rect.right - rect.left) && activeDisplay.Bounds.Height == (rect.bottom - rect.top))
                {
                    GetWindowThreadProcessId(hWnd, out int pid);
                    fullscreenAppName = Process.GetProcessById(pid).ProcessName;
                    fullscreenApp = true;
                }
                else
                {
                    if (fullscreenApp)
                    {
                        fullscreenApp = false;
                    }
                }

                Thread.Sleep(updateInterval);
            }
        }
        #endregion


        #region Cursor Lock service
        private static void CursorLockService()
        {
            Rectangle clipRec = new Rectangle();
            bool flag;

            while (enableCursorLock)
            {
                if ((pauseCL || fullscreenApp) && enableCursorLock)
                {
                    System.Windows.Forms.Cursor.Clip = new Rectangle();
                    flag = true;
                    while (flag && enableCursorLock)
                    {
                        System.Drawing.Point pos = System.Windows.Forms.Cursor.Position;
                        if (pos.X > clipRec.Top && pos.X < clipRec.Width && pos.Y > clipRec.Y && pos.Y < clipRec.Height)
                        {
                            flag = false;
                        }

                        Thread.Sleep(updateInterval);
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

                Thread.Sleep(updateInterval);
            }

            System.Windows.Forms.Cursor.Clip = new Rectangle();
        }
        #endregion

        #region Monitor Dim service
        private static void MonitorDimService()
        {
            var displays = Screen.AllScreens;
            bool dimFlag = false;
            string pName = "";

            while (enableMonitorDim)
            {
                if (fullscreenApp)
                {
                    // Delayed start
                    Thread.Sleep((int)(Properties.Settings.Default.Dim_Delay * 1000));
                    if (fullscreenApp)
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
                            pName = fullscreenAppName;
                            var blacklist = Properties.Settings.Default.Blacklist;

                            if (Properties.Settings.Default.BlacklistMode)  // Blacklist mode
                            {
                                if (blacklist.Count == 0 || !blacklist.Contains(pName.ToLower()))
                                {
                                    Application.Current.Dispatcher.Invoke(() => CreateDimmers(displays));
                                    dimFlag = true;
                                }
                            }
                            else                                            // Whitelist mode
                            {
                                if (blacklist.Count > 0)
                                {
                                    if (blacklist.Contains(pName.ToLower()))
                                    {
                                        Application.Current.Dispatcher.Invoke(() => CreateDimmers(displays));
                                        dimFlag = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (pName != fullscreenAppName)
                    {
                        Application.Current.Dispatcher.Invoke(() => DestroyDimmers());
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

                Thread.Sleep(updateInterval);
            }
        }

        private static Dimmer[] dimmers;

        private static void CreateDimmers(Screen[] displayref)
        {
            dimmers = new Dimmer[displayref.Length];
            for (int i = 0; i < displayref.Length; i++)
            {
                dimmers[i] = new Dimmer()
                {
                    Height = displayref[i].Bounds.Height,
                    Width = displayref[i].Bounds.Width,
                    Top = displayref[i].Bounds.Top,
                    Left = displayref[i].Bounds.Left
                };
            }

            for (int i = 0; i < displayref.Length; i++)
            {
                if (!displayref[i].Equals(activeDisplay))
                {
                    dimmers[i].FadeIn();
                }
            }
        }

        private static void DestroyDimmers()
        {
            if (dimmers != null)
            {
                foreach (var dimmer in dimmers)
                {
                    dimmer.FadeOut();
                }
            }
        }
        #endregion


        #region Controls events
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            App.CloseApp();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(donateURL);

        private void BtnWebsite_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(websiteURL);

        private void TabNavMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}