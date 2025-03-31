
using Notification.Wpf;
using Notification.Wpf.Classes;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Data.Entities;

namespace Notifications
{
    //Not implemented correctly
    public class NotificationService
    {
        private object lockObj = new object();

        private System.Windows.Forms.NotifyIcon notifyIcon;
        // We need state to know what we gonna do: show/hide
        private WindowState storedWindowState;

        private System.Media.SoundPlayer player;

        private Action Show;
        private Action Hide;
        public NotificationService(Action show, Action hide)
        {
            player = new System.Media.SoundPlayer(Environment.CurrentDirectory + "\\notification.wav");
            //player = new System.Media.SoundPlayer(@"c:\Windows\Media\chimes.wav");
            //SendProgramLevelNotification("cj");

            Show = show;
            Hide = hide;

            storedWindowState = WindowState.Normal;

            // Implementing tray icon logic
            // copied from https://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/
            // initialise code here
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Reminder";
            notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            notifyIcon.Click += new EventHandler(notifyIcon_Click);
            notifyIcon.Visible = true;

            // Hiding on startup
            Hide();
            storedWindowState = WindowState.Minimized;
        }

        public async Task SendCustomNotification(string message)
        {
            // Just Random Undertale Reference
            //SendNotication("Reminder", "Chara\nstay determined");\
            //SendProgramLevelNotification("cj");
            SendBaloonTipNotication(message);
        }

        public async Task HandelEventNotifications()
        {
            var db = new Context();
            var events = await GetEventForNow(db);
            
            if (events != null && events.Count > 0)
            {
                await SendEventNotification(events, db);
            }
            else if (DateTime.Now.Minute == 0 && DateTime.Now.Second > 0 && DateTime.Now.Second < 5)
            {
                var f3 = await GetTodaysFirst3Events(db);
            }


            await db.DisposeAsync();
        }

        public void OnCloseTriggered()
        {
            Hide();
            storedWindowState = WindowState.Minimized;
        }

        private async Task<List<Event>> GetEventForNow(Context db)
        {
            var events = await db.Events
                .Where(e => e.TriggerTime.Day == DateTime.Today.Day && e.TriggerTime.Hour == DateTime.Now.Hour && e.TriggerTime.Minute == DateTime.Now.Minute && e.IsEnabled == true && e.Triggered != true)
                .ToListAsync();
            return events;
        }
        private async Task<List<Event>> GetTodaysFirst3Events(Context db)
        {
            var events = await db.Events
                .Where(e => e.TriggerTime.Day == DateTime.Today.Day && e.IsEnabled == true && e.Triggered != true)
                .Take(3)
                .ToListAsync();
            return events;
        }

        private async Task SendEventNotification(List<Event> evList, Context db)
        {
            var sb = new StringBuilder(DateTime.Now.ToString("hh:mm\n"));
            foreach (var e in evList)
            {
                if (e.TriggerTime.Minute == DateTime.Now.Minute)
                {
                    sb.Append(e.Title + "\n");
                    e.Triggered = true;
                }
            }

            await db.SaveChangesAsync();

            SendBaloonTipNotication(sb.ToString());
        }

        // TODO There are bugs here
        private void SendFirst3Notifications(List<Event> f3, Context db)
        {
            if (f3 != null && f3.Count > 0)
            {
                var sb = new StringBuilder("You have work to do today\n");
                foreach (var e in f3)
                {
                    sb.Append(e.Title + "\n");
                }
                SendBaloonTipNotication(sb.ToString());
            }
            else
            {
                SendBaloonTipNotication("Heey!!!\n You have no more work for today");
            }
        }

        private void SendProgramLevelNotification(string message)
        {
            var notificationManager = new NotificationManager();
            var content = new NotificationContent
            {
                Title = "Remionder",
                Message = message,
                Type = NotificationType.Information,
                //TrimType = NotificationTextTrimType.Attach, // will show attach button on message
                RowsCount = 4, //Will show 3 rows and trim after
                //LeftButtonAction = () => SomeAction(), //Action on left button click, button will not show if it null 
                //RightButtonAction = () => SomeAction(), //Action on right button click,  button will not show if it null
                //LeftButtonContent, // Left button content (string or what u want
                //RightButtonContent, // Right button content (string or what u want
                CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

                Background = new SolidColorBrush(Colors.Black),
                Foreground = new SolidColorBrush(Colors.Aqua),

                //Icon = new SvgAwesome()
                //{
                //    Icon = EFontAwesomeIcon.Regular_Star,
                //    Height = 25,
                //    Foreground = new SolidColorBrush(Colors.Yellow)
                //},

                //Image = new NotificationImage()
                //{
                //    Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
                //    Position = ImagePosition.Top
                //}

            };
            notificationManager.Show(content, expirationTime: new TimeSpan(999, 0, 0));
            //player.Play();
        }

        private void SendBaloonTipNotication(string message)
        {
            notifyIcon.BalloonTipText = message;
            notifyIcon.BalloonTipTitle = "Reminder";
            notifyIcon.ShowBalloonTip(2000);
            player.Play();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (storedWindowState != WindowState.Normal)
            {
                Show();
                storedWindowState = WindowState.Normal;
            }

            // Show/Hide Logic
            //if (storedWindowState == WindowState.Minimized)
            //{
            //    Show();
            //    storedWindowState = WindowState.Normal;
            //}
            //else
            //{
            //    Hide();
            //    storedWindowState = WindowState.Minimized;
            //}
        }
    }

}
