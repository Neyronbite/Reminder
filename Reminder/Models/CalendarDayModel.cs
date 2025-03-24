using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Reminder.Models
{
    /// <summary>
    /// Day model will include all events, reminders or notes that user has on that day
    /// </summary>
    public class CalendarDayModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DaysOfWeekEnum DaysOfWeek { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        //TODO alarms
        public List<string> Alarms { get; set; }
        public Brush Background { get; set; }

        public CalendarDayModel()
        {
            Background = Brushes.Cyan;
        }
        // Other properties, like textes alarms etc 
    }
}
