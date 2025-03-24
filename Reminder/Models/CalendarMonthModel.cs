using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Models
{
    /// <summary>
    /// Month model presents full month data, that will be presentet on grids, with day events etc
    /// </summary>
    public class CalendarMonthModel
    {
        public string Title { get; set; }

        public Dictionary<int, CalendarDayModel> DaysDict { get; set; }
        public DaysOfWeekEnum StartDayOfWeek { get; set; }
        public int DaysCount { get; set; }

        public CalendarMonthModel()
        {
            DaysDict = new Dictionary<int, CalendarDayModel>();
        }
    }
}
