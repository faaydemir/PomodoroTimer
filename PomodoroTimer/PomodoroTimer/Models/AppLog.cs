using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace PomodoroTimer.Models
{
    public class AppLog
    {
        public string LogType { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public DateTime Time { get; set; }
        public Guid UserId { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
    }
}
