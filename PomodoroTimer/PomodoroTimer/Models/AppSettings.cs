using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class AppSettings
    {   
        public PomodoroSettings PomodoroSettings { get; set; }
        public UserSettings UserSettings { get; set; }
        public bool KeepStatistic { get; set; }
        public bool SoundAlarm { get; set; }
        public bool VibrationAlarm { get; set; }
    }
}
