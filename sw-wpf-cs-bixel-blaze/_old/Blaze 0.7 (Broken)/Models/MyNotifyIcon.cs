using Blaze.Views;
using Blaze.ViewModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Blaze.Models
{
    public class MyNotifyIcon
    {
        private static NotifyIcon _notifyIcon = new NotifyIcon();
        private static ContextMenu _notifyIconContextMenu = new ContextMenu();

        public MyNotifyIcon()
        {
            _notifyIcon.Icon = new Icon(@"..\..\Blaze\Resources\Icons\Icon_Blaze.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Blaze";
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.ContextMenu = _notifyIconContextMenu;
            _notifyIconContextMenu.MenuItems.Add(new MenuItem(MainViewModel.EnableCursorLock ? "Disable Cursor Lock" : "Enable Cursor Lock", Toggle_CursorLock));
            _notifyIconContextMenu.MenuItems.Add(new MenuItem(MainViewModel.EnableMonitorDim ? "Disable Monitor Dim" : "Enable Monitor Dim", Toggle_MonitorDim));
            _notifyIconContextMenu.MenuItems.Add(new MenuItem("-"));
            _notifyIconContextMenu.MenuItems.Add(new MenuItem("Quit", Notify_Exit));
        }

        public void Notify_Exit(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void Toggle_MonitorDim(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (MainViewModel.EnableMonitorDim)
            {
                MainViewModel.EnableMonitorDim = false;
                menuItem.Text = "Enable Monitor Dim";
            }
            else
            {
                MainViewModel.EnableMonitorDim = true;
                menuItem.Text = "Disable Monitor Dim";
            }
        }

        private void Toggle_CursorLock(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (MainViewModel.EnableCursorLock)
            {
                MainViewModel.EnableCursorLock = false;
                menuItem.Text = "Enable Cursor Lock";
            }
            else
            {
                MainViewModel.EnableCursorLock = true;
                menuItem.Text = "Disable Cursor Lock";
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Show main window
        }
    }
}
