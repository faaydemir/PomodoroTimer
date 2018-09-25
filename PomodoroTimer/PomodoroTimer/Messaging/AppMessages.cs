using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Messaging
{
    public class AppMessages
    {
        public static readonly string CancelledMessage = "CancelledMessage";
        public static readonly string StopTimerService = "StopTimerService";
        public static readonly string StartTimerService = "StartTimerService";
        public static readonly string TimerMessage = "TickedMessage";
        public static readonly string TimerControlMessage = "TimerControlMessage";
        public static readonly string SaveUserTaskMessage = "SaveUserTaskMessage";
        public static readonly string UpdateUserTaskMessage = "UpdateUserTaskMessage";
    }
}
