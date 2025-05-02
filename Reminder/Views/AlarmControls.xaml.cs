using Data.Entities;
using Data;
using Reminder.Models;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Reminder.Utils;
using Reminder.Models.Enums;
using System.Windows;

namespace Reminder.Views
{
    /// <summary>
    /// Interaction logic for AlarmControls.xaml
    /// </summary>
    public partial class AlarmControls : UserControl
    {
        private AlarmModel _alarm;
        private Action<AlarmControls> _deleteCallback;
        public AlarmControls(AlarmModel alarm, Action<AlarmControls> deleteCallback)
        {
            _alarm = alarm;
            _deleteCallback = deleteCallback;
            InitializeComponent();

            var title = new TextBox() { Text = $"{alarm.Title}" };
            title.TextChanged += (s, e) =>
            {
                _alarm.Title = title.Text;
            };

            var tpicker = new TimePicker() { SelectedTime = DateTime.Today.AddHours(alarm.Hour).AddMinutes(alarm.Minute) };
            tpicker.SelectedTimeChanged += (s, e) =>
            {
                _alarm.Hour = tpicker.SelectedTime.Value.Hour;
                _alarm.Minute = tpicker.SelectedTime.Value.Minute;
            };

            var enabled = new CheckBox() { IsChecked = alarm.Enabled, Content = "Enabled" };
            var delete = new Button() { Content = "Delete", Background = Brushes.OrangeRed, BorderBrush = Brushes.OrangeRed, MaxWidth = 100 };
            
            DockPanel.SetDock(enabled, Dock.Bottom);
            DockPanel.SetDock(delete, Dock.Bottom);
            DockPanel.SetDock(title, Dock.Top);
            DockPanel.SetDock(tpicker, Dock.Top);

            enabled.Click += (s, e) =>
            {
                alarm.Enabled = !alarm.Enabled;
            };

            delete.Click += async (s, e) =>
            {
                await StaticDb.SqliteQueries.DeleteIfExists<Alarm>(_alarm.Map<AlarmModel, Alarm>());
                _deleteCallback(this);
            };

            AlarmView.Children.Add(enabled);
            AlarmView.Children.Add(title);
            AlarmView.Children.Add(tpicker);

            var dows = Enum.GetNames(typeof(DaysOfWeekEnum));
            for (int i = 0; i < dows.Length; i++)
            {
                var shortStr = dows[i].ToString().Substring(0, 3);
                var dowCheckBox = new CheckBox() { IsChecked = alarm.DaysOfWeekBits[i], Content = shortStr };

                var tmp = i;
                dowCheckBox.Click += (s, e) =>
                {
                    _alarm.ToggleDayOfWeek((DaysOfWeekEnum)tmp);
                };
                DockPanel.SetDock(dowCheckBox, Dock.Left);
                AlarmView.Children.Add(dowCheckBox);
            }

            AlarmView.Children.Add(delete);
        }
    }
}
