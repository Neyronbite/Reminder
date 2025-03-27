using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Reminder.Models.UIElements
{
    public class GridButton : Button
    {
        public static Brush DefaultBackgroundBrush => Brushes.Transparent;

        //public static readonly DependencyProperty CornerRadiusProperty =
        //    DependencyProperty.Register(
        //        "CornerRadius",
        //        typeof(CornerRadius),
        //        typeof(GridButton),
        //        new FrameworkPropertyMetadata(new CornerRadius(1500)));
        //public CornerRadius CornerRadius
        //{
        //    get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        //    set { SetValue(CornerRadiusProperty, value); }
        //}

        public GridButton(string content, int colGrid, int rowGrid, int fontSize = 24)
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
