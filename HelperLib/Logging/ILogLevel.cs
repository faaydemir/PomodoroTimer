using System;

namespace HelperLib.Log
{
    public interface ILogLevel
    {
        int Priority { get; set; }
        string Name { get; set; }

    }
}