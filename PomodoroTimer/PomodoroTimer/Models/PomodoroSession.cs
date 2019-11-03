using System;
using System.Collections.Generic;

namespace PomodoroTimer.Models
{
    public class PomodoroSession
    {

        public Guid Id { get; set; }
        public int CountToBreak { get; set; } = 4;
        public int FinishedWithoutBreak { get; set; } = 0;
        public List<TaskStatistic> FinishedTaskInfo { get; set; }
        public DateTime Day { get; set; }
        public PomodoroSession()
        {
            Id = Guid.NewGuid();
            Day = DateTime.Now;
            FinishedTaskInfo = new List<TaskStatistic>();
        }

    }
}