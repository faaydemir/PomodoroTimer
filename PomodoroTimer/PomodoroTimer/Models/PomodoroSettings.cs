using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class PomodoroSettings 
    {
        public Guid Id { get; set; }
        public bool AutoContinue { get; set; } = false;
        public int SessionPomodoroCount { get; set; } = 1;
        public int PomodoroBreakDuration { get; set; } = 1;
        public int SessionBreakDuration { get; set; } = 1;
        public int PomodoroDuration { get; set; } = 1;
    }
}
