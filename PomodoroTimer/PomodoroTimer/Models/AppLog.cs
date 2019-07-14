using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace PomodoroTimer.Models
{
    public class AppLog
    {
        public string LogType { get; internal set; }
        public string Message { get; internal set; }
        public string Data { get; internal set; }
        public DateTime Time { get; internal set; }
        public Guid UserId { get; internal set; }
        public DeviceType DeviceType { get; internal set; }
        public string Model { get; internal set; }
        public DevicePlatform Platform { get; internal set; }
    }
}
