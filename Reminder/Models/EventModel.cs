using System;

namespace Reminder.Models
{
    public class EventModel
    {
        public long Id { get; set; }
        public long DayId { get; set; }
        public int DayNum { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Enabled { get; set; }
        public bool Triggered { get; set; }
        public string Title { get; set; }
        public string TriggerTime => new DateTime(Year, Month, DayNum, Hour, Minute, 0).ToString("t");
    }
}
