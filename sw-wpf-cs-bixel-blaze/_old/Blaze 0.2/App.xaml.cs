using System;
using System.Diagnostics;
using System.Drawing;
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
            // Terminate if already running.
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            if (processes.Length > 1)
            {
                CloseApp();
            }


            // NotifyIcon
            notifyIcon.Icon = new Icon(@"..\..\Blaze\Assats\Icons\Icon_Blaze.ico");
            notifyIcon.Visible = true;
            notifyIcon.Text = "Blaze";
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            ContextMenu notifyIconContextMenu = new ContextMenu();
            notifyIcon.ContextMenu = notifyIconContextMenu;
            //notifyIconContextMenu.MenuItems.Add(new MenuItem("Toggle CursorLock"));
            //notifyIconContextMenu.MenuItems.Add(new MenuItem("Toggle Monitor Dim"));
            //notifyIconContextMenu.MenuItems.Add(new MenuItem("Toggle Inactive Dim"));
            //notifyIconContextMenu.MenuItems.Add(new MenuItem("-"));
            notifyIconContextMenu.MenuItems.Add(new MenuItem("Quit", Notify_Exit));


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
    }
}
