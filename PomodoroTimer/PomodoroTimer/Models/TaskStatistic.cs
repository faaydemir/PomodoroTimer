using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class TaskStatistic
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string TaskName { get; set; }
        public string TaskColor { get; set; }//TODO delete if not used.
        public DateTime FinishedTime { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class FinishedTaskStatistic
    {
        public Guid TaskId { get; set; }
        public int DailyFinishedCount { get; set; } = 0;
        public int WeeklyFinishedCount { get; set; } = 0;
        public int MonthlyFinishedCount { get; set; } = 0;
        public int YearlyFinishedCount { get; set; } = 0;

        internal void Add(int count)
        {
            DailyFinishedCount += count;
            WeeklyFinishedCount += count;
            MonthlyFinishedCount += count;
            YearlyFinishedCount += count;

        }
    }
}
