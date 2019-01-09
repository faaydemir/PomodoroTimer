using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public class PomdoroStatus
    {
        public DateTime StartTime { get; set; }
        public PomodoroState PomodoroState { get; set; } = PomodoroState.Ready;
        public TimeSpan RunTime { get; set; } = TimeSpan.Zero;
        public TimeSpan RemainingTime { get; set; } = TimeSpan.Zero;
        public TimerState TimerState { get; set; } = TimerState.Stoped;
    }
}
