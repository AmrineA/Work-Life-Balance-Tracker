using System;

namespace WorkLifeBalanceTracker.Models
{
    public class TimeEntry
    {
        public DateTime StartTime { get; set; }
        public string StartReason { get; set; }
        public DateTime EndTime { get; set; }
        public string EndReason { get; set; }
        public double TotalTime
        {
            get
            {
                return EndTime.Subtract(StartTime).TotalMilliseconds;
            }
        }

        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0, 0, 0, 0, (int)TotalTime);
                return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
            }
        }
    }
}
