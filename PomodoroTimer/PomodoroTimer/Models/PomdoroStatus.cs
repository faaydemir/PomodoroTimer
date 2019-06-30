using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public class PomodoroTimerState
    {
        public DateTime StartTime { get; set; }
        public PomodoroState PomodoroState { get; set; }
        public TimeSpan RunTime { get; set; } 
        public TimeSpan RemainingTime { get; set; } 
        public TimerState TimerState { get; set; }

        public PomodoroTimerState()
        {
            PomodoroState = PomodoroState.Ready;
            RunTime = TimeSpan.Zero;
            RemainingTime = TimeSpan.Zero;
            TimerState = TimerState.Stoped;
        }
    }
}
