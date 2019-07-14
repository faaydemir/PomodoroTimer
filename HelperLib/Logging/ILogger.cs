using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    /// <summary>
    /// ILogger
    /// </summary>
    public interface ILogger
    {
        ILogLevel DefaultLevel { get;}
        void Log(ILogEntity logEntity);
        void Log(Exception ex);
        void Log(ILogLevel level, string message);
        void Log(LogLevelEnum level, string message);
        void Log(string message);
    }
}
