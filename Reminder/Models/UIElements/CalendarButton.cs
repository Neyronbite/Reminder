using System.Windows.Media;

namespace Reminder.Models.UIElements
{
    public class CalendarButton : GridButton
    {
        public static Brush SpecialBackgroundBrush => Brushes.LightBlue;
        public CalendarButton(string content, int colGrid, int rowGrid, bool isSpecial = false, int fontSize = 24) : base (content, colGrid, rowGrid, fontSize)
        {
            if (isSpecial)
            {
                Background = SpecialBackgroundBrush;
            }
        }
    }
}
