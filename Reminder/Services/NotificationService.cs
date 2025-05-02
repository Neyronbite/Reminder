using Microsoft.Toolkit.Uwp.Notifications;
using NotifyIcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.UI.Notifications;
using Data;
using Data.Entities;
using Reminder.Utils;
using Reminder.Models;

namespace Notification
{
    public class NotificationService
    {
        private object lockObj = new object();

        private System.Media.SoundPlayer player;
        private NotifyIconService notifyIconService;

        private Timer timer;
        private int intervalSeconds = 3;
        private int regualrNotificationsPerHour = 1;
        public NotificationService(NotifyIconService notifyIconService)
        {
            this.notifyIconService = notifyIconService;

            // setting timer
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = intervalSeconds * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckForTrigger();
        }

        private async Task CheckForTrigger()
        {
            var today = DateTime.Today;
            var events = await StaticDb.SqliteQueries.GetEvents(today.Month, today.Year, today.Day);
            // time bug
            var toBeTriggered = events.Where(e => e.Triggered != true && e.Enabled == true && (e.Hour < DateTime.Now.Hour || e.Hour == DateTime.Now.Hour && e.Minute <= DateTime.Now.Minute)).ToList();

            if (toBeTriggered != null && toBeTriggered.Count > 0)
            {
                SendEventNotifications(toBeTriggered);
                foreach (var ev in toBeTriggered)
                {
                    ev.Triggered = true;
                    await StaticDb.SqliteQueries.Update(ev);
                }
            }

            // Minute started
            if (DateTime.Now.Second > 0 && DateTime.Now.Second <= intervalSeconds)
            {
                if (DateTime.Now.Minute % (60 / regualrNotificationsPerHour) == 0)
                {
                    var next3 = events.Where(e => e.Triggered != true && e.Enabled == true && (e.Hour > DateTime.Now.Hour || e.Hour == DateTime.Now.Hour && e.Minute > DateTime.Now.Minute)).Take(3).ToList();
                    if (next3 != null && next3.Count > 0)
                    {
                        SendEventNotifications(next3, "For Today");
                    }
                    else
                    {
                        //ShowToast("Nothing planned", "you have nothing else planned today");
                    }
                }

                var todayDow = DateTime.Now.DayOfWeek.Convert();
                var alarmEntities = await StaticDb.SqliteQueries.GetAllAlarms();
                var alarms = alarmEntities.Select(e => e.Map<Alarm, AlarmModel>().Init())
                    .Where(e => e.Enabled == true && e.Hour == DateTime.Now.Hour && e.Minute == DateTime.Now.Minute && e.DaysOfWeekBits[(int)todayDow])
                    .ToList();

                if (alarms != null && alarms.Count > 0)
                {
                    foreach (var item in alarms)
                    {
                        ShowToast("Alarm", item.Title);
                    }
                }
            }
        }

        private void SendEventNotifications(List<Event> events, string heading = null)
        {
            var sb = new StringBuilder();
            foreach (var e in events)
            {
                sb.Append(e.Title + "\n");
            }

            ShowToast(string.IsNullOrWhiteSpace(heading) ? DateTime.Now.ToString("t") : heading, sb.ToString() );
        }

        private void ShowToast(string title, string message)
        {
            try
            {
                ToastContentBuilder builder = new ToastContentBuilder();
                builder.AddArgument("title", "Reminder");
                builder.AddText(title);
                builder.AddText(message);
                builder.AddAppLogoOverride(new Uri(Environment.CurrentDirectory + "\\icon.png"));
                builder.AddAudio(new Uri(Environment.CurrentDirectory + "\\notification.wav"));
                builder.AddButton("Dismiss", new ToastActivationType(), "dismiss");

                builder.Show();

                if (notifyIconService.StoredWindowState == WindowState.Minimized)
                {
                    notifyIconService.ShowRedIcon();
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
