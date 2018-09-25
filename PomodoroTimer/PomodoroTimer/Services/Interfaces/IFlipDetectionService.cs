using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    // will be implemented
    public class DeviceFlipedEventArgs : EventArgs
    {
        public DeviceRotation DeviceRotation { get; set; }
    }
    public delegate void DeviceFlipedEventHandler(object sender, DeviceFlipedEventArgs args);


    public interface IFlipDetectionService
    {
        event DeviceFlipedEventHandler DeviceFlipedEvent;
        TimeSpan StayDuration { get; set; }
    }
}
