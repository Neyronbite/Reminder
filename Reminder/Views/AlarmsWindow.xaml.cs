using Data;
using Data.Entities;
using Reminder.Models;
using Reminder.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reminder.Views
{
    /// <summary>
    /// Interaction logic for AlarmsWindow.xaml
    /// </summary>
    public partial class AlarmsWindow : Window
    {
        List<AlarmModel> alarms;
        public AlarmsWindow()
        {
            InitializeComponent();

            var alarmTask = StaticDb.SqliteQueries.GetAllAlarms();
            Task.WaitAll(alarmTask);
            List<Alarm> alarmEntities = alarmTask.Result;
            alarms = alarmEntities.Select(a => a.Map<Alarm, AlarmModel>().Init()).ToList();
            alarms.ForEach(a => AlarmssListView.Children.Add(new AlarmControls(a, (al) =>
            {
                alarms.Remove(a);
                AlarmssListView.Children.Remove(al);
            })));
        }

        private async void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            foreach (var al in alarms)
            {
                await StaticDb.SqliteQueries.CreateOrUpdate(al.Map<AlarmModel, Alarm>());
            }
            this.Close();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            var alarm = new AlarmModel()
            {
                Enabled = true,
                Hour = 0,
                Minute = 0,
                Title = "Title",
                DaysOfWeek = 0
            };
            alarm.Init();

            alarms.Add(alarm);

            AlarmssListView.Children.Add(new AlarmControls(alarm, a =>
            {
                alarms.Remove(alarm);
                AlarmssListView.Children.Remove(a);
            }));
        }
    }
}
