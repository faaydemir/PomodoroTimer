using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    /// <summary>
    /// LogLevelEnum
    /// </summary>
    public enum LogLevelEnum
    {
        OFF = 0,
        FATAL = 100,
        ERROR = 200,
        WARN = 300,
        INFO = 400,
        DEBUG = 500,
        TRACE = 600,
    }
    public static class LogLevelEnumExtentions
    {
        public static ILogLevel ToLogLevel(this LogLevelEnum logLevelEnum)
        {
            return new LogLevel(logLevelEnum);
        }
    }
}
