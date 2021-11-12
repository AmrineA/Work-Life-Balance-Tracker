using System;

namespace WorkLifeBalanceTracker.Models
{
    public class WeekEntry
    {
        public string Day { get; set; }

        public int Count { get; set; }
        
        public double TotalTime { get; set; }
        
        public string TotalTimeFormatted
        {
            get
            {
                var span = new TimeSpan(0,0,0, (int)TotalTime);
                return $"{Math.Round(span.TotalHours, 2)} Hours";
            }
        }

        public int Progress
        {
            get
            {
                return (int)(TotalTime / 144000 * 100);
            }
        }
    }
}
