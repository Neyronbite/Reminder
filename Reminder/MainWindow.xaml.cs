using Notification;
using NotifyIcon;
using Reminder.Services;
using Reminder.Views;
using System;
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
            try
            {
                InitializeComponent();

                NotifyIconService = new NotifyIconService(Show, Hide);
                NotificationService = new NotificationService(NotifyIconService);
                CalendarService = new CalendarService();

                var initTask = CalendarService.Init(CalendarGrid);
                Task.WaitAll(initTask);
            }
            catch (System.Exception e)
            {
                MessageBox.Show($"An exception occures\n{e.ToString()}");

                throw;
            }
        }

        void OnClose(object sender, CancelEventArgs args)
        {
            NotifyIconService.OnCloseTriggered();
            args.Cancel = true;
        }

        private void Clock_Click(object sender, RoutedEventArgs e)
        {
            var alarmsWindow = new AlarmsWindow();
            alarmsWindow.ShowDialog();
        }
    }
}
