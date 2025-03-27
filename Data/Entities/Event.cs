using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Event : BaseEntity
    {
        public int DayId { get; set; }
        public string Title { get; set; }
        public bool IsEnabled { get; set; }
        public bool Triggered { get; set; }
        public bool Canceled { get; set; }
        public DateTime TriggerTime { get; set; }

        public Day Day { get; set; }
    }
}
