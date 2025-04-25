using Notification;
using NotifyIcon;
using Reminder.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NotificationService NotificationService { get; set; }
        public NotifyIconService NotifyIconService { get; set; }
        public CalendarService CalendarService { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            NotifyIconService = new NotifyIconService(Show, Hide);
            NotificationService = new NotificationService(NotifyIconService);
            CalendarService = new CalendarService();

            var initTask = CalendarService.Init(CalendarGrid);
            Task.WaitAll(initTask);
        }

        void OnClose(object sender, CancelEventArgs args)
        {
            NotifyIconService.OnCloseTriggered();
            //args.Cancel = true;
        }
    }
}
