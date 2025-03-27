using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Models
{
    public class EventModel
    {
        public string Title { get; set; }
        public bool IsEnabled { get; set; }
        public bool Triggered { get; set; }
        public bool Canceled { get; set; }
        public DateTime TriggerTime { get; set; }
    }
}
