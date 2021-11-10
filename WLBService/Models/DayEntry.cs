using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLifeBalanceTracker.Models
{
    public class DayEntry
    {
        public DateTime Date { get; set; }
        public string Title
        {
            get
            {
                return Date.ToString("MM-dd-yyyy");
            }
        }
        public double TotalTime
        {
            get
            {
                return this.TimeEntries.Sum(x => x.TotalTime);
            }
        }
        [JsonIgnore]
        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0,0,0,0, (int)TotalTime);
                return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
            }
        }
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        [JsonIgnore]
        public List<TimeEntry> TimeEntriesFormatted
        {
            get
            {
                return TimeEntries.OrderByDescending(e => e.StartTime).ToList();
            }
        }
    }
}
