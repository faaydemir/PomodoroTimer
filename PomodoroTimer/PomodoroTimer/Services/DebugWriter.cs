using HelperLib.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public class DebugWriter : ILogWriter
    {
        public void Write(ILogEntity logEntity)
        {
            System.Diagnostics.Debug.WriteLine(logEntity.ToString());
        }
    }
}
