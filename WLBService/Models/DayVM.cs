using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLifeBalanceTracker.Models;

namespace WorkLifeBalanceTracker.Models
{
    public class DayVM
    {
        public string Title { get; set; }

        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0, 0, 0, 0, (int)TimeEntries.Sum(e => e.TotalTime));
                return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
            }
        }
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}
