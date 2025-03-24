using Reminder.Models;
using Reminder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Reminder
{
    public class Calendar
    {
        private int _year;
        private int _month;
        private CalendarMonthModel calendarMonthModel;

        public Calendar()
        {

        }

        public void Init(Grid grid)
        {
            _month = DateTime.Today.Month;
            _year = DateTime.Today.Year;
            calendarMonthModel = GetMonthCalendar(_year, _month);

            FillGrid(grid, calendarMonthModel);
        }

        public void FillGrid(Grid grid, CalendarMonthModel monthModel)
        {
            grid.Children.Clear();

            List<Button> buttons = new List<Button>();

            int row = 2;
            int col = (int)monthModel.StartDayOfWeek;

            for (int i = 1; i <= monthModel.DaysCount; i++)
            {
                var btn = new Button()
                {
                    Content = i.ToString(),
                    //VerticalAlignment = VerticalAlignment.Top
                    VerticalContentAlignment = VerticalAlignment.Top,
                };
                Grid.SetColumn(btn, col);
                Grid.SetRow(btn, row);
                btn.Click += Btn_Click;

                if (monthModel.DaysDict.TryGetValue(i, out CalendarDayModel cdm))
                {
                    //TODO add whole attributes, text, alarm info etc to button click
                    //TODO add button click handler that opens new window with all info about that day
                    //btn.Background = Brushes.IndianRed;
                    btn.Background = cdm.Background;
                    btn.Content += $"\n{cdm.Title}";
                }

                col++;
                if (col > 6)
                {
                    col = 0;
                    row++;
                }

                buttons.Add(btn);
                grid.Children.Add(btn);
            }
        }

        public CalendarMonthModel GetMonthCalendar(int year, int month)
        {
            DateTime monthStart = new DateTime(year, month, 1);

            var calendarMonthModel = new CalendarMonthModel();
            calendarMonthModel.Title = monthStart.Month.ToString();
            calendarMonthModel.StartDayOfWeek = monthStart.DayOfWeek.Convert();
            calendarMonthModel.DaysCount = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);

            var today = new CalendarDayModel()
            {
                Day = DateTime.Today.Day,
                Month = month,
                DaysOfWeek = DateTime.Today.DayOfWeek.Convert(),
                Title = "Today"
            };

            if (DateTime.Now.Year == year && DateTime.Now.Month == month)
            {
                calendarMonthModel.DaysDict.Add(today.Day, today);
            }

            //TODO get all data for days

            return calendarMonthModel;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var currentBtn = sender as Button;

            var dayStr = currentBtn?.Content?.ToString()?.Split('\n')[0];
            var day = int.Parse(dayStr);

            CalendarDayModel dayModel;
            if (calendarMonthModel.DaysDict.ContainsKey(day))
            {
                dayModel = calendarMonthModel.DaysDict[day];
            }
            else
            {
                dayModel = new CalendarDayModel();
                dayModel.Day = day;
                dayModel.Month = _month;
                dayModel.Year = _year;

                calendarMonthModel.DaysDict.Add(day, dayModel);
            }

            var wnd = new DayDetails(dayModel, cdm =>
            {
                currentBtn.Content = $"{day}\n{cdm.Title}";
                currentBtn.Background = cdm.Background;
                calendarMonthModel.DaysDict[day] = cdm;
            });
            wnd.ShowDialog();
        }
    }
}
