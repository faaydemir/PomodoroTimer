using HelperLib.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public class DebugLogger : Logger
    {
        public DebugLogger() : base(new DebugWriter())
        {
        }
    }
}
