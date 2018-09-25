using System;
using System.Collections.Generic;

namespace PomodoroTimer
{
    public class PomodoroSession
    {
        public int CountToBreak { get; set; }
        public int FinishedWithoutBreak { get; set; }
        public int DayTotal { get; set; }
        public List<Guid> FinishedTasks { get; set; }
        public DateTime Day { get; set; }
    }
}