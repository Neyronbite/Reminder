using System.Collections.Generic;
using Reminder.Models.Enums;

namespace Reminder.Models
{
    /// <summary>
    /// Month model presents full month data, that will be presentet on grids, with day events etc
    /// </summary>
    public class MonthModel
    {
        public string Title { get; set; }

        public Dictionary<int, DayModel> DaysDict { get; set; }
        public DaysOfWeekEnum StartDayOfWeek { get; set; }
        public int DaysCount { get; set; }

        public MonthModel()
        {
            DaysDict = new Dictionary<int, DayModel>();
        }
    }
}
