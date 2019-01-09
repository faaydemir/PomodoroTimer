using System;
using System.Collections.Generic;

namespace PomodoroTimer.Models
{
    public class PomodoroSession
    {
        public PomodoroSession()
        {
            Id = Guid.NewGuid();
            //FinishedTasks = new List<Guid>();
            FinishedTaskInfo = new List<TaskStatistic>();
        }

        public Guid Id { get; set; }
        public int CountToBreak { get; set; } = 4;
        public int FinishedWithoutBreak { get; set; } = 0;
        //public int DayTotalFinished { get; set; } = 0;
        //public int DayTotalFinishedDuration { get; set; } = 0;
        //public int DayTotalCanceled { get; set; } = 0;
        public List<TaskStatistic> FinishedTaskInfo { get; set; }
        //public List<Guid> FinishedTasks { get; set; }
        public DateTime Day { get; set; } = DateTime.Today;


    }
}