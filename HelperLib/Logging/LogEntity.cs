using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    public class LogEntity : ILogEntity
    {
        #region props
        public DateTime Time { get; set; }
        public ILogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string CallerName { get; set; }
        public string Scope { get; set; }
        #endregion

        #region ctor
        public LogEntity(string message,
                         ILogLevel logLevel = null,
                         DateTime? time = null,
                         string scope = null,
                         string callerName = null)
        {
            Message = message;
            LogLevel = logLevel;
            Time = time ?? DateTime.Now;
            Scope = scope;
            CallerName = callerName;
        }
        public LogEntity(string message,
                 LogLevelEnum logLevel = LogLevelEnum.OFF,
                 DateTime? time = null,
                 string scope = null,
                 string callerName = null)
        {
            Message = message;
            LogLevel = logLevel.ToLogLevel();
            Time = time ?? DateTime.Now;
            Scope = scope;
            CallerName = callerName;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(Time);
            stringBuilder.Append('\t');


            stringBuilder.Append(LogLevel);
            stringBuilder.Append('\t');

            if (!string.IsNullOrWhiteSpace(Scope))
            {
                stringBuilder.Append(Scope);
                stringBuilder.Append('\t');
            }

            if (!string.IsNullOrWhiteSpace(CallerName))
            {
                stringBuilder.Append(CallerName);
                stringBuilder.Append('\t');
            }

            stringBuilder.Append(Message);
            stringBuilder.Append('\t');

            return stringBuilder.ToString();
        }
    }
}
