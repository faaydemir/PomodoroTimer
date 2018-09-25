using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public class TimerInfo
    {
        public TimeSpan RunTime { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public TimerState TimerState { get; set;}
        public PomodoroState PomodoroState { get; set; }
    }
}
