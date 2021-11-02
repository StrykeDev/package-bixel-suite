using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;


namespace Blaze
{
    public partial class App : Application
    {
        public static MainWindow mainWindow = new MainWindow();
        public static NotifyIcon notifyIcon = new NotifyIcon();


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // NotifyIcon
            notifyIcon.Icon = new Icon(@"..\..\Blaze\Assats\Icons\Icon_Blaze.ico");
            notifyIcon.Visible = true;
            notifyIcon.Text = "Blaze";
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            ContextMenu notifyIconContextMenu = new ContextMenu();
            notifyIcon.ContextMenu = notifyIconContextMenu;
            notifyIconContextMenu.MenuItems.Add(new MenuItem(mainWindow.EnableCursorLock ? "Disable Cursor Lock" : "Enable Cursor Lock", Toggle_CursorLock));
            notifyIconContextMenu.MenuItems.Add(new MenuItem(mainWindow.EnableMonitorDim ? "Disable Monitor Dim" : "Enable Monitor Dim", Toggle_MonitorDim));
            notifyIconContextMenu.MenuItems.Add(new MenuItem("-"));
            notifyIconContextMenu.MenuItems.Add(new MenuItem("Quit", Notify_Exit));


            // Terminate if already running.
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            if (processes.Length > 1)
            {
                notifyIcon.ShowBalloonTip(3, "OOF", "Blaze already running!", ToolTipIcon.Error);
                Thread.Sleep(3000);
                CloseApp();
            }

            // Show main window
            if (!Blaze.Properties.Settings.Default.Start_Min)
            {
                mainWindow.Show();
            }
        }


        // App events
        public static void CloseApp()
        {
            notifyIcon.Dispose();
            Current.Shutdown();
            Environment.Exit(0);
        }


        // NotifyIcon events
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            mainWindow.Show();
        }

        private void Notify_Exit(object sender, EventArgs e)
        {
            CloseApp();
        }

        private void Toggle_CursorLock(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (mainWindow.EnableCursorLock)
            {
                mainWindow.EnableCursorLock = false;
                menuItem.Text = "Enable Cursor Lock";
            }
            else
            {
                mainWindow.EnableCursorLock = true;
                menuItem.Text = "Disable Cursor Lock";
            }
        }

        private void Toggle_MonitorDim(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (mainWindow.EnableMonitorDim)
            {
                mainWindow.EnableMonitorDim = false;
                menuItem.Text = "Enable Monitor Dim";
            }
            else
            {
                mainWindow.EnableMonitorDim = true;
                menuItem.Text = "Disable Monitor Dim";
            }
        }
    }
}
