using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

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
        public static string DonateURL => @"https://www.bixel.io/donate?amount=1";
        public static string WebsiteURL => @"https://www.bixel.io";
        public static string VersionName => "Alpha Build";

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
        public static bool fullscreenApp = false;
        public static string fullscreenAppName = "";
        public static System.Windows.Forms.Screen activeDisplay;

        private static void ActiveAppsService()
        {
            RECT rect = new RECT();
            IntPtr hWnd;

            while (true)
            {
                hWnd = GetForegroundWindow();

                GetWindowRect(new HandleRef(null, hWnd), ref rect);
                activeDisplay = System.Windows.Forms.Screen.FromHandle(hWnd);

                if (activeDisplay.Bounds.Width == (rect.right - rect.left) && activeDisplay.Bounds.Height == (rect.bottom - rect.top))
                {
                    if (!fullscreenApp)
                    {
                        fullscreenApp = true;
                        fullscreenAppName = hWnd.ToString();
                    }
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
            var displays = System.Windows.Forms.Screen.AllScreens;
            Dimmer[] dimmers = new Dimmer[displays.Length];
            for (int i = 0; i < dimmers.Length; i++)
            {
                dimmers[i] = new Dimmer()
                {
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Height = displays[i].Bounds.Height,
                    Width = displays[i].Bounds.Width,
                    Top = displays[i].Bounds.Top,
                    Left = displays[i].Bounds.Left,
                    Opacity = 0.1
                };
            }
            bool dimFlag = false;

            while (enableMonitorDim)
            {
                if (fullscreenApp)
                {
                    if (!displays.Equals(System.Windows.Forms.Screen.AllScreens)) // Check if displays changed, if so reset dimmers
                    {
                        displays = System.Windows.Forms.Screen.AllScreens;

                        for (int i = 0; i < dimmers.Length; i++)
                        {
                            dimmers[i].Close();
                        }

                        dimmers = new Dimmer[displays.Length];
                        for (int i = 0; i < dimmers.Length; i++)
                        {
                            dimmers[i] = new Dimmer()
                            {
                                Height = displays[i].Bounds.Height,
                                Width = displays[i].Bounds.Width,
                                Top = displays[i].Bounds.Top,
                                Left = displays[i].Bounds.Left,
                                Opacity = 0
                            };
                        }

                        dimFlag = false;
                    }

                    if (!dimFlag)
                    {
                        for (int i = 0; i < displays.Length; i++)
                        {
                            if (!displays[i].Equals(activeDisplay))
                            {
                                dimmers[i].Show();
                            }
                        }

                        dimFlag = true;
                    }
                }
                else
                {
                    if (dimFlag)
                    {
                        for (int i = 0; i < dimmers.Length; i++)
                        {
                            dimmers[i].Hide();
                        }

                        dimFlag = false;
                    }
                }

                Thread.Sleep(updateInterval);
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

        private void BtnDonate_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(DonateURL);
        private void BtnWebsite_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(WebsiteURL);
        #endregion
    }
}