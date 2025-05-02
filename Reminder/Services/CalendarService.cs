using Data;
using Data.Entities;
using Reminder.Models;
using Reminder.Models.Enums;
using Reminder.Models.UIElements;
using Reminder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Reminder.Services
{
    public class CalendarService
    {
        // Current calendar page's month and year
        protected int year;
        protected int month;
        // Current calendar month model
        protected MonthModel monthModel;
        // Calendar Grid
        protected Grid grid;

        /// <summary>
        /// Getting calendar data, and initializing UI
        /// </summary>
        /// <param name="grid">Calendar grid</param>
        public async Task Init(Grid grid)
        {
            // Setting todays year and month
            month = DateTime.Today.Month;
            year = DateTime.Today.Year;

            this.grid = grid;

            // Setting this month's days
            monthModel = await GetMonthData(year, month);

            // On startup filling UI with current month model
            FillGridWithCurrentMonth();
        }

        /// <summary>
        /// Filling calendar grid UI with calendar buttons and month model data
        /// </summary>
        protected virtual void FillGridWithCurrentMonth()
        {
            // Clearing everithing in calendar grid
            grid.Children.Clear();

            // Day buttons list
            List<Button> buttons = new List<Button>();

            FillStaticUIElements();

            // Filling calendar day buttons
            int row = 2;
            int col = (int)monthModel.StartDayOfWeek;

            for (int i = 1; i <= monthModel.DaysCount; i++)
            {
                Button btn;

                // If we have something on that day in our month model, we are marking it as special
                if (monthModel.DaysDict.TryGetValue(i, out DayModel cdm))
                {
                    btn = new CalendarButton($"{i}\n{cdm.Title}", col, row, true, backColor: cdm.GetColor());
                }
                // Eles, just common button
                else
                {
                    btn = new CalendarButton(i.ToString(), col, row);
                }
                btn.Click += Btn_Click;

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

        /// <summary>
        /// Fills calendar's days of week, left and right buttons, and central text
        /// </summary>
        protected virtual void FillStaticUIElements()
        {
            // Go to previous month logic
            var leftBtn = new GridButton("<", 0, 0, 36);
            leftBtn.Click += async (s, e) =>
            {
                if (month - 1 < 1)
                {
                    month = 12;
                    year--;
                }
                else
                {
                    month--;
                }

                this.monthModel = await GetMonthData(year, month);
                FillGridWithCurrentMonth();
            };
            grid.Children.Add(leftBtn);

            // Go to next month logic
            var rightBtn = new GridButton(">", 6, 0, 36);
            rightBtn.Click += async (s, e) =>
            {
                if (month + 1 > 12)
                {
                    month = 1;
                    year++;
                }
                else
                {
                    month++;
                }

                this.monthModel = await GetMonthData(year, month);
                FillGridWithCurrentMonth();
            };
            grid.Children.Add(rightBtn);

            // Just text of month and year
            var dateTextBlock = new TextBlock()
            {
                Text = $"{year} {(MonthsEnum)month}",
                FontSize = 36,
                TextAlignment = TextAlignment.Center,
            };
            Grid.SetColumn(dateTextBlock, 2);
            Grid.SetColumnSpan(dateTextBlock, 3);
            Grid.SetRow(dateTextBlock, 0);
            grid.Children.Add(dateTextBlock);

            // Filling days of week
            for (int i = 0; i < 7; i++)
            {
                var dowBtn = new GridButton(((DaysOfWeekEnum)i).ToString(), i, 1);
                grid.Children.Add(dowBtn);
            }
        }

        /// <summary>
        /// Getting month's data from db, and creatin new MonthModel
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<MonthModel> GetMonthData(int year, int month)
        {
            DateTime monthStart = new DateTime(year, month, 1);

            var monthModel = new MonthModel();
            monthModel.Title = monthStart.Month.ToString();
            monthModel.StartDayOfWeek = monthStart.DayOfWeek.Convert();
            monthModel.DaysCount = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);

            // Getting data for days from db
            var dayEntities = await StaticDb.SqliteQueries.GetDays(month, year);
            monthModel.DaysDict = dayEntities
                .Select(d => d.Map<Day, DayModel>())
                .ToDictionary(d => d.DayNum);

            if (monthModel.DaysDict == null)
            {
                monthModel.DaysDict = new Dictionary<int, DayModel> ();
            }

            if (DateTime.Now.Year == year && DateTime.Now.Month == month && !monthModel.DaysDict.ContainsKey(DateTime.Today.Day))
            {
                var today = new DayModel()
                {
                    DayNum = DateTime.Today.Day,
                    Month = month,
                    DayOfWeek = DateTime.Today.DayOfWeek.Convert(),
                    Title = "Today",
                    Year = DateTime.Today.Year,
                };

                monthModel.DaysDict.Add(today.DayNum, today);
            }

            return monthModel;
        }

        /// <summary>
        /// Handling month day buttons click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var currentBtn = sender as CalendarButton;

            // Getting clicked day
            var dayStr = currentBtn?.Content?.ToString()?.Split('\n')[0];
            var day = int.Parse(dayStr);

            DayModel dayModel;
            // Checking, if we already have something on that day
            if (monthModel.DaysDict.ContainsKey(day))
            {
                dayModel = monthModel.DaysDict[day];
            }
            // Else, creating new day model
            else
            {
                dayModel = new DayModel();
                dayModel.DayNum = day;
                dayModel.Month = month;
                dayModel.Year = year;
            }

            // Oppening new day details editor window
            var wnd = new DayDetails(dayModel,
                // Callback on applaying changes on that day
                cdm =>
                {
                    if (monthModel.DaysDict.ContainsKey(day))
                    {
                        monthModel.DaysDict[day] = cdm;
                    }
                    else
                    {
                        monthModel.DaysDict.Add(day, cdm);
                    }
                    currentBtn.Content = $"{day}\n{cdm.Title}";

                    // Setting background colors
                    currentBtn.BackColor = cdm.GetColor();
                    currentBtn.Background = cdm.GetColor() != null ? 
                        new SolidColorBrush((System.Windows.Media.Color)cdm.GetColor()) : 
                        CalendarButton.SpecialBackgroundBrush;
                });
            wnd.ShowDialog();
        }
    }
}
