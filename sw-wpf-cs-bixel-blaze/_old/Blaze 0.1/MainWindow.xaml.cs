using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Blaze
{
    public partial class Main : Window
    {
        public static string donateURL = @"https://www.bixel.io/donate?amount=1";
        public static string websiteURL = @"https://www.bixel.io";
        public static string versionName = "Test Build 2019";
        public static bool loaded = false;
        public static int interval = Properties.Settings.Default.Update_Interval;
        public static bool startMinimized = Properties.Settings.Default.Start_Min;
        public static bool enableCursorLock = Properties.Settings.Default.Enable_CursorLock;
        public static bool enableMonitorDim = Properties.Settings.Default.Enable_MonitorDim;


        public static bool pauseServices = false;
        public static bool stopServices = false;
        
        public static Thread thHotkeys = new Thread(HotKeysService);
        public static Thread thCursorLock = new Thread(CursorLockService);
        public static Thread thMonitorDim = new Thread(MonitorDimService);

        public RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


        public Main()
        {
            InitializeComponent();

            // Load settings
            cbxStartMin.IsChecked = Properties.Settings.Default.Start_Min;
            cbxCursorLock.IsChecked = Properties.Settings.Default.Enable_CursorLock;
            cbxMonitorDim.IsChecked = Properties.Settings.Default.Enable_MonitorDim;            
            cbxStartup.IsChecked = reg.GetValue("Blaze") != null;

            // Threading
            thHotkeys.SetApartmentState(ApartmentState.STA);
            thHotkeys.Start();


            // Loaded
            loaded = true;

            CbxCursorLock_Checked(null, null);
            CbxMonitorDim_Checked(null, null);
        }

        public void UpdateStatusMsg(string msg)
        {
            lblStatus.Content = msg;
        }



        // Imports
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



        // Services
        private static void HotKeysService()
        {
            while (true)
            {
                if (Keyboard.IsKeyToggled(Key.Scroll))
                {
                    pauseServices = true;
                    stopServices = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    pauseServices = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    pauseServices = true;
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    pauseServices = true;
                }
                else
                {
                    pauseServices = false;
                }

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) 
                {
                    if (Keyboard.IsKeyDown(Key.Add))
                    {
                        // decresse opacity
                    }
                    else if (Keyboard.IsKeyDown(Key.Subtract))
                    {
                        // incresse opacity
                    }
                    else if (Keyboard.IsKeyDown(Key.Multiply))
                    {
                        // toggle monitor dimming
                    }
                }

                Thread.Sleep(interval);
            }
        }

        private static void CursorLockService()
        {
            Rectangle clipRec = new Rectangle();

            System.Drawing.Point pos;
            bool flag;

            while (Properties.Settings.Default.Enable_CursorLock)
            {
                if (pauseServices)
                {
                    Console.WriteLine("unlock");
                    System.Windows.Forms.Cursor.Clip = new Rectangle();
                    flag = true;

                    do
                    {
                        pos = System.Windows.Forms.Cursor.Position;

                        if (pos.X > clipRec.Top && pos.X < clipRec.Width && pos.Y > clipRec.Y && pos.Y < clipRec.Height)
                        {
                            flag = false;
                        }

                        Thread.Sleep(interval);
                    } while (flag && pauseServices || pauseServices);
                }
                else
                {
                    clipRec.Height = Properties.Settings.Default.CursorLock_Height;
                    clipRec.Width = Properties.Settings.Default.CursorLock_Width;
                    clipRec.X = Properties.Settings.Default.CursorLock_X;
                    clipRec.Y = Properties.Settings.Default.CursorLock_Y;

                    System.Windows.Forms.Cursor.Clip = clipRec;
                }

                Thread.Sleep(interval);
            }

            System.Windows.Forms.Cursor.Clip = new Rectangle();
        }

        private static void MonitorDimService()
        {
            bool dimFlag = false;
            RECT rect = new RECT();
            System.Windows.Forms.Screen[] displays = System.Windows.Forms.Screen.AllScreens;
            Dimmer[] dimmers = new Dimmer[displays.Length];

            for (int i = 0; i < dimmers.Length; i++)
            {
                dimmers[i] = new Dimmer()
                {
                    Height = displays[i].Bounds.Height,
                    Width = displays[i].Bounds.Width,
                    Top = displays[i].Bounds.Top,
                    Left = displays[i].Bounds.Left,
                    Opacity = Properties.Settings.Default.Dim_Opacity
                };
            }

            while (Properties.Settings.Default.Enable_MonitorDim)
            {
                IntPtr hWnd = GetForegroundWindow();
                GetWindowRect(new HandleRef(null, hWnd), ref rect);

                System.Windows.Forms.Screen activeDisplay = System.Windows.Forms.Screen.FromHandle(hWnd);

                if (activeDisplay.Bounds.Width == (rect.right - rect.left) && activeDisplay.Bounds.Height == (rect.bottom - rect.top))
                {
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
                
                Thread.Sleep(interval);
            }
        }



        // Controls events
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure that you want to terminate Blaze?", "Blaze", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Hide();
                App.CloseApp();
            }
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }


        private void CbxCursorLock_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Properties.Settings.Default.Enable_CursorLock = cbxCursorLock.IsChecked.Value;
                Properties.Settings.Default.Save();

                if (cbxCursorLock.IsChecked.Value)
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
            else
            {
                cbxCursorLock.IsChecked = Properties.Settings.Default.Enable_CursorLock;
            }
        }

        private void CbxMonitorDim_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Properties.Settings.Default.Enable_MonitorDim = cbxMonitorDim.IsChecked.Value;
                Properties.Settings.Default.Save();

                if (cbxMonitorDim.IsChecked.Value)
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
            else
            {
                cbxMonitorDim.IsChecked = Properties.Settings.Default.Enable_MonitorDim;
            }
        }

        private void CbxStartMin_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Properties.Settings.Default.Start_Min = cbxStartMin.IsChecked.Value;
                Properties.Settings.Default.Save();
            }
        }

        private void CbxStartup_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                if (cbxStartup.IsChecked.Value)
                {
                    string str = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    reg.SetValue("Blaze", '"' + str + '"');
                }
                else
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.DeleteValue("Blaze");
                }

            }
        }


        private void CursorLock_Updated(object sender, RoutedEventArgs e)
        {
            cbxCursorLock.IsChecked = false;
            cbxCursorLock.IsChecked = true;
        }

        private void MonitorDim_Updated(object sender, RoutedEventArgs e)
        {
            cbxMonitorDim.IsChecked = false;
            cbxMonitorDim.IsChecked = true;
        }

        private void InactiveDim_Updated(object sender, RoutedEventArgs e)
        {

        }

        
        private void BtnDonate_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(donateURL);

        private void BtnWebsite_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(websiteURL);
    }
}
