using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerService.Models
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
        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0, 0, 0, 0, (int)this.TimeEntriesFormatted.Sum(x => x.TotalTime));
                return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
            }
        }
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        public List<TimeEntry> TimeEntriesFormatted
        {
            get
            {
                if (Date == DateTime.Today && State.TimeEntry != null)
                {
                    var entry = new TimeEntry()
                    {
                        StartTime = State.TimeEntry.StartTime,
                        StartReason = State.TimeEntry.StartReason,
                    };
                    return TimeEntries.Union(new List<TimeEntry>() { entry }).OrderByDescending(e => e.StartTime).ToList();
                }
                else
                    return TimeEntries.OrderByDescending(e => e.StartTime).ToList();
            }
        }
    }
}
