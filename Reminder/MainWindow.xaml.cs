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

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer GlobalTimer { get; set; }
        public Calendar Calendar { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitTimer();
            Calendar = new Calendar();
            Calendar.Init(CalendarGrid);
        }

        private void InitTimer()
        {
            GlobalTimer = new System.Timers.Timer();
            GlobalTimer.AutoReset = true;
            GlobalTimer.Interval = 1000;
            GlobalTimer.Elapsed += GlobalTimer_Elapsed;
            GlobalTimer.Start();
        }

        private void GlobalTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {

        }
    }
}

// TODO
// X button: close program to taskbar icon
// Sqlite db
// Startup program on installation
// Change program icon
// 