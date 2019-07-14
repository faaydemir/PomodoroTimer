using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    public interface ILogEntity
    {
        ILogLevel LogLevel { get; set; }
        string Message { get; set; }
        string CallerName { get; set; }
        string Scope { get; set; }

        DateTime Time { get; set; }
    }
}
