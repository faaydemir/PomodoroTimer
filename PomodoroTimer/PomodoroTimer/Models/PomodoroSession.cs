using System;
using System.Collections.Generic;

namespace PomodoroTimer.Models
{
    public class PomodoroSession
    {
        public Guid Id { get; set; }
        public int CountToBreak { get; set; } = 4;
        public int FinishedWithoutBreak { get; set; } = 0;
        public int DayTotalFinished { get; set; } = 0;
        public int DayTotalFinishedDuration { get; set; } = 0;
        public int DayTotalCanceled { get; set; } = 0;
        public List<TaskStatistic> FinishedTaskInfo { get; set; } = new List<TaskStatistic>();
        public List<Guid> FinishedTasks { get; set; } = new List<Guid>();
        public DateTime Day { get; set; } = DateTime.Today;


        public PomodoroSession()
        {
            Id = Guid.NewGuid();
            FinishedTasks = new List<Guid>();
        }
    }
}