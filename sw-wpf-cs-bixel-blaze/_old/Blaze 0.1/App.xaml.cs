using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace Blaze
{
    public partial class App : Application
    {
        public static System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public static Main mainWindow = new Main();


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Terminate if already running.
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1)
            {
                MessageBox.Show("Blaze is already running.", "Blaze",MessageBoxButton.OK);
                CloseApp();
            }


            // NotifyIcon
            notifyIcon.Icon = new Icon(@"..\..\Blaze\Assats\Icons\Icon_Blaze.ico");
            notifyIcon.Visible = true;
            notifyIcon.Text = "Blaze";
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            System.Windows.Forms.ContextMenu notifyIconContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon.ContextMenu = notifyIconContextMenu;

            notifyIconContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Toggle CursorLock"));
            notifyIconContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Toggle Monitor Dim"));
            notifyIconContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Toggle Inactive Dim"));
            notifyIconContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
            notifyIconContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Quit", Notify_Exit));


            // Show main window
            if (!Blaze.Properties.Settings.Default.Start_Min)
            {
                mainWindow.Show();
            }
        }

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
    }
}
