using Data.Entities;
using Reminder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Utils
{
    public static class ObjectMapper
    {
        public static DayModel ToModel(this Day day)
        {
            var dayModel = new DayModel()
            {
                Title = day.Title,
                Notes = day.Notes,
                Year = day.Year,
                Month = day.Month,
                Day = day.DayNum,
                DayOfWeek = (Models.Enums.DaysOfWeekEnum)day.DayOfWeek
            };

            return dayModel;
        }

        public static Day ToEntity(this DayModel dayModel)
        {
            var day = new Day()
            {
                Title = dayModel.Title,
                Notes = dayModel.Notes,
                Year = dayModel.Year,
                Month = dayModel.Month,
                DayNum = dayModel.Day,
                DayOfWeek = (int)dayModel.DayOfWeek
            };

            return day;
        }

        public static EventModel ToModel(this Event @event)
        {
            var eventModel = new EventModel()
            {
                Title = @event.Title,
                IsEnabled = @event.IsEnabled,
                Triggered = @event.Triggered,
                Canceled = @event.Canceled,
                TriggerTime = @event.TriggerTime,
            };

            return eventModel;
        }

        public static Event ToEntity(this EventModel eventModel, int dayId)
        {
            var @event = new Event()
            {
                DayId = dayId,
                Title = eventModel.Title,
                IsEnabled = eventModel.IsEnabled,
                Triggered = eventModel.Triggered,
                Canceled = eventModel.Canceled,
                TriggerTime = eventModel.TriggerTime,
            };

            return @event;
        }

        public static List<Event> ToEntity(this List<EventModel> eventModels, int dayId)
        {
            return eventModels.Select(e => e.ToEntity(dayId)).ToList();
        }

        public static List<EventModel> ToModel(this List<Event> events)
        {
            return events.Select(e => e.ToModel()).ToList();
        }
    }
}
