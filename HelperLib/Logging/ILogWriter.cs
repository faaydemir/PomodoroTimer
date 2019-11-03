using System;
using System.Collections.Generic;
using System.Text;

namespace HelperLib.Log
{
    public interface ILogWriter
    {



        void Write(ILogEntity logEntity);
    }
}
