using Data;
using Microsoft.EntityFrameworkCore;
using Reminder.Models;
using Reminder.Models.Enums;
using Reminder.Models.UIElements;
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
        // Current calendar page's month and year
        private int _year;
        private int _month;
        // Current calendar month model
        private MonthModel monthModel;

        public Calendar()
        {

        }

        /// <summary>
        /// Getting calendar data, and initializing UI
        /// </summary>
        /// <param name="grid">Calendar grid</param>
        public async Task Init(Grid grid)
        {
            // Setting todays year and month
            _month = DateTime.Today.Month;
            _year = DateTime.Today.Year;
            // Setting this month's days
            monthModel = await GetMonthData(_year, _month);

            // On startup filling UI with current month model
            FillGrid(grid, monthModel);
        }

        /// <summary>
        /// Filling calendar grid UI with calendar buttons and month model data
        /// </summary>
        /// <param name="grid">Calendar grid</param>
        /// <param name="monthModel">Month model</param>
        public void FillGrid(Grid grid, MonthModel monthModel)
        {
            // Clearing everithing in calendar grid
            grid.Children.Clear();

            // Day buttons list
            List<Button> buttons = new List<Button>();

            // Go to previous month logic
            var leftBtn = new GridButton("<", 0, 0, 36);
            leftBtn.Click += async (s, e) =>
            {
                if (_month - 1 < 1)
                {
                    _month = 12;
                    _year--;
                }
                else
                {
                    _month--;
                }

                this.monthModel = await GetMonthData(_year, _month);
                FillGrid(grid, this.monthModel);
            };
            grid.Children.Add(leftBtn);

            // Go to next month logic
            var rightBtn = new GridButton(">", 6, 0, 36);
            rightBtn.Click += async (s, e) =>
            {
                if (_month + 1 > 12)
                {
                    _month = 1;
                    _year++;
                }
                else
                {
                    _month++;
                }

                this.monthModel = await GetMonthData(_year, _month);
                FillGrid(grid, this.monthModel);
            };
            grid.Children.Add(rightBtn);

            // Just text of month and year
            var dateTextBlock = new TextBlock()
            {
                Text = $"{_year} {(MonthsEnum)_month}",
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

            // Filling calendar day buttons
            int row = 2;
            int col = (int)monthModel.StartDayOfWeek;

            for (int i = 1; i <= monthModel.DaysCount; i++)
            {
                Button btn;

                // If we have something on that day in our month model, we are marking it as special
                if (monthModel.DaysDict.TryGetValue(i, out DayModel cdm))
                {
                    //TODO add whole attributes, text, alarm info etc to button click
                    //TODO add button click handler that opens new window with all info about that day
                    //btn.Background = Brushes.IndianRed;
                    btn = new CalendarButton($"{i}\n{cdm.Title}", col, row, true);
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
            // Maybe it will be more correct and more practical to use something like Services in asp core
            // but
            // I don't give a fuck
            // I use db everywhere
            var db = new Context();
            var dayModels = await db.Days
                .Where(d => d.Month == month && d.Year == year && d.IsDeleted != true)
                .Select(d => d.ToModel())
                .ToDictionaryAsync(d => d.Day);
            monthModel.DaysDict = dayModels;

            foreach (var item in monthModel.DaysDict)
            {
                item.Value.Events = await db.Events
                    .Where(e => e.DayId == item.Value.Id && e.IsDeleted != true)
                    .Select(e => e.ToModel())
                    .ToListAsync();
            }

            await db.DisposeAsync();

            if (DateTime.Now.Year == year && DateTime.Now.Month == month && !monthModel.DaysDict.ContainsKey(DateTime.Today.Day))
            {
                var today = new DayModel()
                {
                    Day = DateTime.Today.Day,
                    Month = month,
                    DayOfWeek = DateTime.Today.DayOfWeek.Convert(),
                    Title = "Today"
                };

                monthModel.DaysDict.Add(today.Day, today);
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
            var day = int.Parse(dayStr!);

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
                dayModel.Day = day;
                dayModel.Month = _month;
                dayModel.Year = _year;
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
                    currentBtn!.Content = $"{day}\n{cdm.Title}";
                    currentBtn.Background = CalendarButton.SpecialBackgroundBrush;
                });
            wnd.ShowDialog();
        }
    }
}
