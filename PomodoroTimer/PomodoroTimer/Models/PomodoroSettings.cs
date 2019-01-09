using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PomodoroTimer.Models
{
    public class PomodoroSettings 
    {
        public Guid Id { get; set; }
        public bool AutoContinue { get; set; } = false;
        public int SessionPomodoroCount { get; set; } = 4;
        public TimeSpan PomodoroBreakDuration { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan SessionBreakDuration { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan PomodoroDuration { get; set; } = TimeSpan.FromMinutes(25);

    }
}
