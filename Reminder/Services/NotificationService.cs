using Microsoft.Toolkit.Uwp.Notifications;
using NotifyIcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.UI.Notifications;

namespace Notification
{
    public class NotificationService
    {
        private object lockObj = new object();

        private System.Media.SoundPlayer player;
        private NotifyIconService notifyIconService;
        public NotificationService(NotifyIconService notifyIconService)
        {
            //player = new System.Media.SoundPlayer(Environment.CurrentDirectory + "\\notification.wav");
            this.notifyIconService = notifyIconService;

            ShowToast("Testing Toast", "aasdasdasdasdasdasd");
        }

        //public async Task HandelEventNotifications()
        //{
        //    var db = new Context();
        //    var events = await GetEventForNow(db);

        //    if (events != null && events.Count > 0)
        //    {
        //        await SendEventNotification(events, db);
        //    }
        //    else if (DateTime.Now.Minute == 0 && DateTime.Now.Second > 0 && DateTime.Now.Second < 5)
        //    {
        //        var f3 = await GetTodaysFirst3Events(db);
        //    }


        //    await db.DisposeAsync();
        //}


        //private async Task<List<Event>> GetEventForNow(Context db)
        //{
        //    var events = await db.Events
        //        .Where(e => e.TriggerTime.Day == DateTime.Today.Day && e.TriggerTime.Hour == DateTime.Now.Hour && e.TriggerTime.Minute == DateTime.Now.Minute && e.IsEnabled == true && e.Triggered != true)
        //        .ToListAsync();
        //    return events;
        //}
        //private async Task<List<Event>> GetTodaysFirst3Events(Context db)
        //{
        //    var events = await db.Events
        //        .Where(e => e.TriggerTime.Day == DateTime.Today.Day && e.IsEnabled == true && e.Triggered != true)
        //        .Take(3)
        //        .ToListAsync();
        //    return events;
        //}

        //private async Task SendEventNotification(List<Event> evList, Context db)
        //{
        //    var sb = new StringBuilder(DateTime.Now.ToString("hh:mm\n"));
        //    foreach (var e in evList)
        //    {
        //        if (e.TriggerTime.Minute == DateTime.Now.Minute)
        //        {
        //            sb.Append(e.Title + "\n");
        //            e.Triggered = true;
        //        }
        //    }

        //    await db.SaveChangesAsync();

        //    SendBaloonTipNotication(sb.ToString());
        //}

        //// TODO There are bugs here
        //private void SendFirst3Notifications(List<Event> f3, Context db)
        //{
        //    if (f3 != null && f3.Count > 0)
        //    {
        //        var sb = new StringBuilder("You have work to do today\n");
        //        foreach (var e in f3)
        //        {
        //            sb.Append(e.Title + "\n");
        //        }
        //        SendBaloonTipNotication(sb.ToString());
        //    }
        //    else
        //    {
        //        SendBaloonTipNotication("Heey!!!\n You have no more work for today");
        //    }
        //}

        //private void SendProgramLevelNotification(string message)
        //{
        //    var notificationManager = new NotificationManager();
        //    var content = new NotificationContent
        //    {
        //        Title = "Remionder",
        //        Message = message,
        //        Type = NotificationType.Information,
        //        //TrimType = NotificationTextTrimType.Attach, // will show attach button on message
        //        RowsCount = 4, //Will show 3 rows and trim after
        //        //LeftButtonAction = () => SomeAction(), //Action on left button click, button will not show if it null 
        //        //RightButtonAction = () => SomeAction(), //Action on right button click,  button will not show if it null
        //        //LeftButtonContent, // Left button content (string or what u want
        //        //RightButtonContent, // Right button content (string or what u want
        //        CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

        //        Background = new SolidColorBrush(Colors.Black),
        //        Foreground = new SolidColorBrush(Colors.Aqua),

        //        //Icon = new SvgAwesome()
        //        //{
        //        //    Icon = EFontAwesomeIcon.Regular_Star,
        //        //    Height = 25,
        //        //    Foreground = new SolidColorBrush(Colors.Yellow)
        //        //},

        //        //Image = new NotificationImage()
        //        //{
        //        //    Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
        //        //    Position = ImagePosition.Top
        //        //}

        //    };
        //    notificationManager.Show(content, expirationTime: new TimeSpan(999, 0, 0));
        //    //player.Play();
        //}

        
        private void ShowToast(string title, string message)
        {
            try
            {
                ToastContentBuilder builder = new ToastContentBuilder();
                builder.AddArgument("title", title);
                builder.AddText("test");
                builder.AddText(message);
                builder.AddAppLogoOverride(new Uri(Environment.CurrentDirectory + "\\icon.png"));
                builder.AddAudio(new Uri(Environment.CurrentDirectory + "\\notification.wav"));
                builder.AddButton("Dismiss", new ToastActivationType(), "dismiss");

                builder.Show();
                //player.Play();

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
