using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using Blaze.Views;
using Blaze.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace Blaze
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public static NotifyIcon _notifyIcon;
        public static MainView _mainView;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Properties
            if (Blaze.Properties.Settings.Default.Blacklist == null)
            {
                Blaze.Properties.Settings.Default.Blacklist = new System.Collections.Specialized.StringCollection();
            }

            // NotifyIcon
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon(@"..\..\Blaze\Resources\Icons\Icon_Blaze.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Blaze";
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // Terminate if already running.
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            if (processes.Length > 1)
            {
                _notifyIcon.ShowBalloonTip(3, "OOF", "Blaze already running!", ToolTipIcon.Error);
                Thread.Sleep(3000);
                CloseApp(true);
            }

            // Show main window
            _mainView = new MainView();
            if (!Blaze.Properties.Settings.Default.Start_Min)
            {
                _mainView.Show();
            }
        }


        public static void CloseApp(bool force)
        {
            if (force)
            {
                _notifyIcon.Dispose();
                Environment.Exit(0);
            }
            else
            {
                var result = MessageBox.Show("Are you sure?", "Blaze", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    _notifyIcon.Dispose();
                    Environment.Exit(0);
                }
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            _mainView.Show();
        }
    }
}
