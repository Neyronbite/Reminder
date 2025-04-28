using Data;
using Data.Entities;
using Reminder.Models;
using Reminder.Models.UIElements;
using Reminder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reminder.Views
{
    /// <summary>
    /// Interaction logic for EventControls.xaml
    /// </summary>
    public partial class EventControls : UserControl
    {
        private EventModel _event;
        private Action<EventControls> _deleteCallback;
        public EventControls(EventModel @event, Action<EventControls> deleteCallback)
        {
            _event = @event;
            _deleteCallback = deleteCallback;

            InitializeComponent();

            var title = new TextBlock() { Text = $"{@event.Title} - {@event.TriggerTime}\t\t" };
            //var time = new TextBlock() { Text =  };
            var enabled = new CheckBox() { IsChecked = @event.Enabled, Content = "Enabled" };
            var delete = new Button() { Content = "Delete", Background = Brushes.OrangeRed, BorderBrush = Brushes.OrangeRed };

            delete.HorizontalAlignment = HorizontalAlignment.Right;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            enabled.HorizontalAlignment = HorizontalAlignment.Center;

            enabled.Click += (s, e) =>
            {
                _event.Enabled = !_event.Enabled;
            };

            delete.Click += async (s, e) =>
            {
                await StaticDb.SqliteQueries.DeleteIfExists<Event>(_event.Map<EventModel, Event>());
                _deleteCallback(this);
            };

            EventView.Children.Add(title);
            //EventView.Children.Add(time);
            EventView.Children.Add(enabled);
            EventView.Children.Add(delete);
        }
    }
}
