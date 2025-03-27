using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer GlobalTimer { get; set; }
        public Calendar Calendar { get; set; }

        private DateTime currentDay;

        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();

            currentDay = DateTime.Now;

            InitTrayIcon();

            InitTimer();
            Calendar = new Calendar();
            Calendar.Init(CalendarGrid);
        }

        private void InitTimer()
        {
            GlobalTimer = new System.Timers.Timer();
            GlobalTimer.AutoReset = true;
            GlobalTimer.Interval = 5000;
            GlobalTimer.Elapsed += GlobalTimer_Elapsed;
            GlobalTimer.Start();
        }

        private void InitTrayIcon()
        {
            // initialise code here
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.Text = "Reminder";
            m_notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
            m_notifyIcon.Visible = true;
        }

        private void GlobalTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            // TODO on day change, update calendar
            // TODO check todays events

            //m_notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            //m_notifyIcon.BalloonTipTitle = "Reminder";
            //m_notifyIcon.ShowBalloonTip(2000);
        }

        // Implementing tray icon logic
        // copied from https://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/
        void OnClose(object sender, CancelEventArgs args)
        {
            Hide();
            m_storedWindowState = WindowState.Minimized;
            //args.Cancel = true;
        }
        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            if (m_storedWindowState == WindowState.Minimized)
            {
                Show();
                m_storedWindowState = WindowState.Normal;
            }
            else
            {
                Hide();
                m_storedWindowState = WindowState.Minimized;
            }
        }

    }
}

// TODO
// X button: close program to taskbar icon
// Sqlite db
// Startup program on installation
// Change program icon
// 