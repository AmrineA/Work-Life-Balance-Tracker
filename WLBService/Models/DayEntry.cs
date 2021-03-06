using System;

namespace WorkLifeBalanceTracker.Models
{
    public class DayEntry
    {
        public string Day { get; set; }

        public int Count { get; set; }
        
        public double TotalTime { get; set; }
        
        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0,0,0, (int)TotalTime);
                return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
            }
        }

        public int Progress
        {
            get
            {
                return (int)(TotalTime / 28800 * 100);
            }
        }
    }
}
