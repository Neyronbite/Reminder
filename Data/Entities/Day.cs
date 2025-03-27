using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Day : BaseEntity
    {
        [Required]
        public int DayNum { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int DayOfWeek { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }

        public List<Event> Events { get; set; }
    }
}
