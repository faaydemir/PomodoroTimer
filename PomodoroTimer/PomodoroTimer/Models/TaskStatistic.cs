using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class TaskStatistic
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime FinishedTime { get; set; }
        public int Duration { get; set; }
    }

    public class FinishedTaskStatistic
    {
        public Guid TaskId { get; set; }
        public int DailyFinishedCount { get; set; } = 0;
        public int WeeklyFinishedCount { get; set; } = 0;
        public int MountlyFinishedCount { get; set; } = 0;
        public int YearlyFinishedCount { get; set; } = 0;

        internal void Add(int count)
        {
            DailyFinishedCount += count;
            WeeklyFinishedCount += count;
            MountlyFinishedCount += count;
            YearlyFinishedCount += count;

        }
    }
}
