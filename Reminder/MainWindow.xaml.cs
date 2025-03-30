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
using Notifications;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer GlobalTimer { get; set; }
        public Calendar Calendar { get; set; }
        public NotificationService NotificationService { get; set; }

        private DateTime currentDay;


        public MainWindow()
        {
            InitializeComponent();

            currentDay = DateTime.Now;

            NotificationService = new NotificationService(Show, Hide);

            InitTimer();
            Calendar = new Calendar();
            var initTask = Calendar.Init(CalendarGrid);
            Task.WaitAll(initTask);
        }

        private void InitTimer()
        {
            GlobalTimer = new System.Timers.Timer();
            GlobalTimer.AutoReset = true;
            GlobalTimer.Interval = 5000;
            GlobalTimer.Elapsed += GlobalTimer_Elapsed;
            GlobalTimer.Start();
        }

        private void GlobalTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            // TODO on day change, update calendar
            // TODO check todays events
            NotificationService.SendCustomNotification();
        }

        // Implementing tray icon logic
        // copied from https://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/
        void OnClose(object sender, CancelEventArgs args)
        {
            NotificationService.OnCloseTriggered();
            //args.Cancel = true;
        }

    }
}

// TODO
// X button: close program to taskbar icon ++
// Sqlite db ++
// Startup program on installation
// Change program icon ++
// appconfig.json for local options, like username etc
// Dark theme