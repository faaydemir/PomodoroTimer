using System;

namespace HelperLib.Log
{
    public class LogLevel : ILogLevel
    {
        #region props

        public int Priority { get; set; }
        public string Name { get; set; }

        #endregion

        #region ctor

        public LogLevel(string name, int priority)
        {

            Priority = priority;
            Name = name;
        }

        public LogLevel(LogLevelEnum logLevel) : this(logLevel.ToString(), (int)logLevel)
        {
        }

        #endregion

        #region cast
        //public static implicit operator LogLevel(LogLevelEnum logLevel)
        //{
        //    return new LogLevel(logLevel);
        //}
        #endregion

        #region statics
        public static ILogLevel OFF { get => LogLevelEnum.OFF.ToLogLevel(); }
        public static ILogLevel FATAL { get => LogLevelEnum.FATAL.ToLogLevel(); }
        public static ILogLevel ERROR { get => LogLevelEnum.ERROR.ToLogLevel(); }
        public static ILogLevel WARN { get => LogLevelEnum.WARN.ToLogLevel(); }
        public static ILogLevel INFO { get => LogLevelEnum.INFO.ToLogLevel(); }
        public static ILogLevel DEBUG { get => LogLevelEnum.DEBUG.ToLogLevel(); }
        public static ILogLevel TRACE { get => LogLevelEnum.TRACE.ToLogLevel(); }

        #endregion

        #region override
        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }
}
