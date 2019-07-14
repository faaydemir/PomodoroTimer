using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    public class Logger : ILogger
    {
        #region props
        public ILogWriter LogWriter { get; }
        public ILogLevel DefaultLevel { get; }

        #endregion

        #region public methods
        public Logger(ILogWriter logWriter, LogLevel defaultLevel = null)
        {
            DefaultLevel = defaultLevel ?? LogLevel.OFF;
            LogWriter = logWriter;
        }

        public void Log(ILogEntity logEntity)
        {
            WriteLog(logEntity);
        }

        //TODO implement
        public void Log(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Log(ILogLevel level, string message)
        {
            Log(new LogEntity(message, level));
        }

        public void Log(LogLevelEnum level, string message)
        {
            Log(new LogEntity(message, level));
        }

        public void Log(string message)
        {
            Log(new LogEntity(message, this.DefaultLevel));
        }
        #endregion

        #region private methods
        private void WriteLog(ILogEntity logEntity)
        {
            LogWriter.Write(logEntity);
        }
        #endregion
    }
}
