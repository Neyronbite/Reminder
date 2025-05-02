using Reminder.Models.Enums;
using System.Linq;
using System.Windows.Media;

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
        public string Color { get; set; }

        public Color? GetColor()
        {
            if (Color == null)
                return null;

            var rgb = Color.Split(',').Select(c => byte.Parse(c)).ToList();
            var color = new Color()
            {
                R = rgb[0],
                G = rgb[1],
                B = rgb[2],
                A = 255
            };
            return color;
        }
    }
}
