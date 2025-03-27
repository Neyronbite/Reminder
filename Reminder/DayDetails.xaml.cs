using Reminder.Models;
using Reminder.Models.Enums;
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
    /// Interaction logic for DayDetails.xaml
    /// </summary>
    public partial class DayDetails : Window
    {
        // Current day model
        DayModel model;
        // Callback, to change UI
        Action<DayModel> applyCallback;

        public DayDetails(DayModel dayModel, Action<DayModel> applyCallback)
        {
            InitializeComponent();

            this.model = dayModel;
            this.applyCallback = applyCallback;

            TitleTextBlox.Text = dayModel.Title;
            NotesTextBlox.Text = dayModel.Notes;
            DateTextBlock.Text = $"{dayModel.Day} {(MonthsEnum)dayModel.Month} {dayModel.Year}";

            if (model.Events == null)
            {
                model.Events = new List<EventModel>();
            }
            else
            {
                foreach (var item in model.Events)
                {
                    //TODO change this later
                    //EventsGrid.Items.Add(item);
                    EventsListView.Items.Add(item);
                }
            }
            //TODO add alarm data
        }

        private void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            model.Title = TitleTextBlox.Text;
            model.Notes = NotesTextBlox.Text;
            //model.Alarms = TODO

            //TODO save to db

            applyCallback(model);
            this.Close();
        }

        private void Button_Click_Add_Reminder(object sender, RoutedEventArgs e)
        {
            var ef = new EventForm(em => 
            {
                var newEv = new EventModel
                {
                    Title = em.Title,
                    TriggerTime = new DateTime(model.Year, model.Month, model.Day)
                };
                newEv.TriggerTime.AddHours(em.TriggerTime.Hour);
                newEv.TriggerTime.AddMinutes(em.TriggerTime.Minute);

                model.Events.Add(newEv);
                //TODO change this later
                //EventsGrid.Children.Add(new TextBlock() { Text = $"{newEv.Title}: {newEv.TriggerTime}" });
                EventsListView.Items.Add(newEv);
            }); 
            ef.ShowDialog();
        }
    }
}
