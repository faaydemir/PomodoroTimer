using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class Pomodoro 
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public PomodoroState State { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
