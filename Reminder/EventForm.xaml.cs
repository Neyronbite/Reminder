using Reminder.Models;
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
using System.Windows.Shapes;

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
            em.TriggerTime = DateTime.Today;
            //TODO fix this shit
            em.TriggerTime = (DateTime)TimePicker.SelectedTime;

            callback(em);   
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
