using Reminder.Models;
using System;
using System.Windows;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for EventForm.xaml
    /// </summary>
    public partial class EventForm : Window
    {
        Action<EventModel> callback;
        public EventForm(Action<EventModel> callback)
        {
            InitializeComponent();

            this.callback = callback;
        }

        private void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            var em = new EventModel();
            em.Title = TitleTextBox.Text;
            em.Hour = TimePicker.SelectedTime.Value.Hour;
            em.Minute = TimePicker.SelectedTime.Value.Minute;

            callback(em);   
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
