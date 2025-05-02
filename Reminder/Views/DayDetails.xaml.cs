using Data;
using Data.Entities;
using Reminder.Models;
using Reminder.Models.Enums;
using Reminder.Models.UIElements;
using Reminder.Utils;
using Reminder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for DayDetails.xaml
    /// </summary>
    public partial class DayDetails : Window
    {
        // Current day model
        DayModel model;
        // Events for current day
        List<EventModel> events;
        // Callback, to change UI
        Action<DayModel> applyCallback;

        public DayDetails(DayModel dayModel, Action<DayModel> applyCallback)
        {
            InitializeComponent();

            this.model = dayModel;
            this.applyCallback = applyCallback;

            TitleTextBlox.Text = dayModel.Title;
            NotesTextBlox.Text = dayModel.Notes;
            DateTextBlock.Text = $"{dayModel.DayNum} {(MonthsEnum)dayModel.Month} {dayModel.Year}";

            var eventsTask = StaticDb.SqliteQueries.GetEvents(dayModel.Month, dayModel.Year, dayModel.DayNum);
            Task.WaitAll(eventsTask);
            var eventEntities = eventsTask.Result;
            events = eventEntities.Select(e => e.Map<Event, EventModel>()).ToList();
            events.ForEach(e => EventsListView.Children.Add(new EventControls(e, (ec) => {
                events.Remove(e);
                EventsListView.Children.Remove(ec);
            })));

            
            ColorPicker.Color = dayModel.GetColor() != null ? (System.Windows.Media.Color)dayModel.GetColor() : CalendarButton.SpecialBackgroundColor;
        }

        private async void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            model.Title = TitleTextBlox.Text;
            model.Notes = NotesTextBlox.Text;

            // getting color
            string rgb = ColorPicker.Color.R.ToString();
            rgb += ',' + ColorPicker.Color.G.ToString();
            rgb += ',' + ColorPicker.Color.B.ToString();
            model.Color = rgb;

            // Updating/creating day model
            model = (await StaticDb.SqliteQueries.CreateOrUpdate(model.Map<DayModel, Day>())).Map<Day, DayModel>();

            // Updating/creating events
            foreach (var ev in events)
            {
                ev.DayId = model.Id;
                await StaticDb.SqliteQueries.CreateOrUpdate(ev.Map<EventModel, Event>());
            }

            // Calling UI callback
            applyCallback(model);
            this.Close();
        }

        private void Button_Click_Add_Reminder(object sender, RoutedEventArgs e)
        {
            var ef = new EventForm(em =>
            {
                em.Year = model.Year;
                em.Month = model.Month;
                em.DayNum = model.DayNum;
                em.Enabled = true;
                events.Add(em);

                EventsListView.Children.Add(new EventControls(em, (ec) => {
                    events.Remove(em);
                    EventsListView.Children.Remove(ec);
                }));
            });
            ef.ShowDialog();
        }
    }
}
