using System.Windows.Controls;
using System.Windows.Media;

namespace Reminder.Models.UIElements
{
    public class GridButton : Button
    {
        public static Brush DefaultBackgroundBrush => Brushes.Transparent;

        public GridButton(string content, int colGrid, int rowGrid, int fontSize = 24, Brush backgroundBrush = null)
        {
            Content = content;
            VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
            FontSize = fontSize;

            Grid.SetColumn(this, colGrid);
            Grid.SetRow(this, rowGrid);

            BorderBrush = DefaultBackgroundBrush;
            Background = DefaultBackgroundBrush;
            //CornerRadius = new CornerRadius(1500);
        }
    }
}
