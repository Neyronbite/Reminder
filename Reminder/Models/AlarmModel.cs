using Reminder.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Models
{
    public class AlarmModel
    {
        public long Id { get; set; }
        public int DaysOfWeek { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Enabled { get; set; }
        public string Title { get; set; }

        public bool[] DaysOfWeekBits { get; set; }

        public AlarmModel Init()
        {
            // 0, 1, 2, 4, 8, 16, 32, 64
            DaysOfWeekBits = new bool[7];
            var dowNum = DaysOfWeek;
            for (int i = 0; i < 7; i++)
            {
                DaysOfWeekBits[i] = Convert.ToBoolean(dowNum % 2);
                dowNum = dowNum / 2;
            }
            return this;
        }

        public void ToggleDayOfWeek(DaysOfWeekEnum daysOfWeek)
        {
            DaysOfWeekBits[(int)daysOfWeek] = !DaysOfWeekBits[(int)daysOfWeek];
            var tempDOW = 0;
            for (int i = 0; i < DaysOfWeekBits.Length; i++)
            {
                tempDOW += (int)Math.Pow(2, i) * Convert.ToInt32(DaysOfWeekBits[i]);
            }
            DaysOfWeek = tempDOW;
        }
    }
}
