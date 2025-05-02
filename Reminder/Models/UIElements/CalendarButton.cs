using System.Windows.Media;

namespace Reminder.Models.UIElements
{
    public class CalendarButton : GridButton
    {
        public static Brush SpecialBackgroundBrush => Brushes.LightBlue;
        public static Color SpecialBackgroundColor => Brushes.LightBlue.Color;
        public Color? BackColor { get; set; }
        public CalendarButton(string content, int colGrid, int rowGrid, bool isSpecial = false, int fontSize = 24, Color? backColor = null) : base (content, colGrid, rowGrid, fontSize)
        {
            if (isSpecial)
            {
                BackColor = backColor;
                Background = BackColor != null ? new SolidColorBrush((Color)BackColor) : SpecialBackgroundBrush;
            }
        }
    }
}
