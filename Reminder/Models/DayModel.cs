using Reminder.Models.Enums;

namespace Reminder.Models
{
    /// <summary>
    /// Day model will include all events, reminders or notes that user has on that day
    /// </summary>
    public class DayModel
    {
        public long Id { get; set; }
        public int DayNum { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DaysOfWeekEnum DayOfWeek { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        //public List<EventModel> Events { get; set; }
    }
}
