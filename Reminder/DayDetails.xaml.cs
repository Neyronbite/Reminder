using Data;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Reminder.Models;
using Reminder.Models.Enums;
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
        }

        private async void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            model.Title = TitleTextBlox.Text;
            model.Notes = NotesTextBlox.Text;

            // Okay, this is real grdon,
            // but it is what it is
            // Saving day data to db
            var db = new Context();
            var dayEntity = await db.Days.Where(d => d.Id == model.Id).FirstOrDefaultAsync();

            if (dayEntity != null)
            {
                dayEntity.Title = TitleTextBlox.Text;
                dayEntity.Notes = NotesTextBlox.Text;
                await db.SaveChangesAsync();

                foreach (var ev in model.Events)
                {
                    var evEntity = await db.Events.Where(e => e.Id == ev.Id).FirstOrDefaultAsync();
                    if (evEntity != null)
                    {
                        evEntity.Title = ev.Title;
                        evEntity.IsEnabled = ev.IsEnabled;
                        evEntity.Canceled = ev.Canceled;
                        evEntity.TriggerTime = ev.TriggerTime;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        evEntity = ev.ToEntity(dayEntity.Id);
                        await db.Events.AddAsync(evEntity);
                        await db.SaveChangesAsync();
                    }
                    ev.Id = evEntity.Id;
                }
            }
            else
            {
                dayEntity = model.ToEntity();
                await db.Days.AddAsync(dayEntity);
                await db.SaveChangesAsync();

                dayEntity.Events = new List<Data.Entities.Event>();

                foreach (var ev in model.Events)
                {
                    var evEntity = ev.ToEntity(dayEntity.Id);
                    await db.Events.AddAsync(evEntity);
                    await db.SaveChangesAsync();

                    ev.Id = evEntity.Id;
                    dayEntity.Events.Add(evEntity);
                }
            }

            await db.DisposeAsync();

            model.Id = dayEntity.Id;

            // Calling UI callback
            applyCallback(model);
            this.Close();
        }

        private void Button_Click_Add_Reminder(object sender, RoutedEventArgs e)
        {
            var ef = new EventForm(em => 
            {
                var dt = new DateTime(model.Year, model.Month, model.Day);

                var newEv = new EventModel
                {
                    Title = em.Title,
                    TriggerTime = new DateTime(model.Year, model.Month, model.Day),
                    IsEnabled = true,
                    Triggered = false,
                };
                newEv.TriggerTime = newEv.TriggerTime
                    .AddHours(em.TriggerTime.Hour)
                    .AddMinutes(em.TriggerTime.Minute);

                model.Events.Add(newEv);
                //TODO change this later
                //EventsGrid.Children.Add(new TextBlock() { Text = $"{newEv.Title}: {newEv.TriggerTime}" });
                EventsListView.Items.Add(newEv);
            }); 
            ef.ShowDialog();
        }
        // TODO Better UI
        // TODO Better event management: delete, enable/disable, change time ...
        // TODO Notes input does not support shift+enter
    }
}
