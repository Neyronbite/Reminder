using Reminder.Models.Enums;
using System;

namespace Reminder.Utils
{
    public static class DayOFWeekUtil
    {
        public static DaysOfWeekEnum Convert(this DayOfWeek dayOfWeek)
        {
            int dow = (int)dayOfWeek - 1;
            if (dow == -1)
            {
                dow = 6;
            }

            return (DaysOfWeekEnum)dow;
        }

        public static DaysOfWeekEnum Next(this DaysOfWeekEnum dayOfWeek)
        {
            if ((int)dayOfWeek < 0 || (int) dayOfWeek > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }

            int next = (int)dayOfWeek + 1;

            if (next > 6)
            {
                next = 0;
            }

            return (DaysOfWeekEnum)next;
        }
    }
}
