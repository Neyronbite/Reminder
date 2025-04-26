using System;
using System.Windows;
using System.Windows.Forms;

namespace NotifyIcon
{
    public class NotifyIconService
    {

        public WindowState StoredWindowState { get => storedWindowState; }

        private System.Windows.Forms.NotifyIcon notifyIcon;
        // We need state to know what we gonna do: show/hide
        private WindowState storedWindowState;
        private Action Show;
        private Action Hide;

        public NotifyIconService(Action show, Action hide)
        {
            Show = show;
            Hide = hide;

            storedWindowState = WindowState.Normal;

            // Implementing tray icon logic
            // copied from https://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/
            // initialise code here
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Reminder";
            notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            notifyIcon.Click += new EventHandler(notifyIcon_Click);
            notifyIcon.Visible = true;

            // setting context menu of icon with close btn
            var menuItem = new MenuItem("Close");
            menuItem.Click += MenuItem_Click;
            notifyIcon.ContextMenu = new ContextMenu();
            notifyIcon.ContextMenu.MenuItems.Add(menuItem);

            // Hiding on startup
            Hide();
            storedWindowState = WindowState.Minimized;
        }

        public void OnCloseTriggered()
        {
            Hide();
            storedWindowState = WindowState.Minimized;
        }

        public void ShowRedIcon()
        {
            notifyIcon.Icon = new System.Drawing.Icon("icon-red.ico");
        }
        public void ShowNormalIcon()
        {
            notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
        }

        private void SendBaloonTipNotication(string message)
        {
            notifyIcon.BalloonTipText = message;
            notifyIcon.BalloonTipTitle = "Reminder";
            notifyIcon.ShowBalloonTip(2000);
        }
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowNormalIcon();
            if (storedWindowState != WindowState.Normal)
            {
                Show();
                storedWindowState = WindowState.Normal;
            }
        }
        private void MenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
