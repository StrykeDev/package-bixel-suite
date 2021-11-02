using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using Prism.Views;
using Prism.ViewModels;

namespace Prism
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public static MainView _mainView = new MainView();
        public static NotifyIcon _notifyIcon = new NotifyIcon();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // NotifyIcon
            _notifyIcon.Icon = new Icon(@"..\..\Prism\Resources\Icons\Icon_Prism.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Prism";
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            ContextMenu notifyIconContextMenu = new ContextMenu();
            _notifyIcon.ContextMenu = notifyIconContextMenu;
            //notifyIconContextMenu.MenuItems.Add(new MenuItem(MainViewModel.EnableCursorLock ? "Disable Cursor Lock" : "Enable Cursor Lock", Toggle_CursorLock));
            //notifyIconContextMenu.MenuItems.Add(new MenuItem(MainViewModel.EnableMonitorDim ? "Disable Monitor Dim" : "Enable Monitor Dim", Toggle_MonitorDim));
            //notifyIconContextMenu.MenuItems.Add(new MenuItem("-"));
            notifyIconContextMenu.MenuItems.Add(new MenuItem("Quit", Notify_Exit));


            // Terminate if already running.
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            if (processes.Length > 1)
            {
                _notifyIcon.ShowBalloonTip(3, "OOF", "Prism already running!", ToolTipIcon.Error);
                Thread.Sleep(3000);
                CloseApp();
            }


            // Show main window
            if (!Prism.Properties.Settings.Default.Start_Min)
            {
                _mainView.Show();
            }
        }


        // App events
        public static void CloseApp()
        {
            _notifyIcon.Dispose();
            Environment.Exit(0);
        }


        // NotifyIcon events
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            _mainView.Show();
        }

        private void Notify_Exit(object sender, EventArgs e)
        {
            CloseApp();
        }

        //private void Toggle_CursorLock(object sender, EventArgs e)
        //{
        //    MenuItem menuItem = sender as MenuItem;

        //    if (MainViewModel.EnableCursorLock)
        //    {
        //        MainViewModel.EnableCursorLock = false;
        //        menuItem.Text = "Enable Cursor Lock";
        //    }
        //    else
        //    {
        //        MainViewModel.EnableCursorLock = true;
        //        menuItem.Text = "Disable Cursor Lock";
        //    }
        //}

        //private void Toggle_MonitorDim(object sender, EventArgs e)
        //{
        //    MenuItem menuItem = sender as MenuItem;

        //    if (MainViewModel.EnableMonitorDim)
        //    {
        //        MainViewModel.EnableMonitorDim = false;
        //        menuItem.Text = "Enable Monitor Dim";
        //    }
        //    else
        //    {
        //        MainViewModel.EnableMonitorDim = true;
        //        menuItem.Text = "Disable Monitor Dim";
        //    }
        //}
    }
}
