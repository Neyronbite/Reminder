using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Reminder.Models.Enums;

namespace Reminder.Models
{
    /// <summary>
    /// Day model will include all events, reminders or notes that user has on that day
    /// </summary>
    public class DayModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DaysOfWeekEnum DayOfWeek { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public List<EventModel> Events { get; set; }

        // Other properties, like textes alarms etc 
    }
}
