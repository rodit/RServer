using System;
using System.Collections.Generic;
using System.Text;

namespace RUtils.Logging
{
    public enum Level
    {
        None = 0,
        Info = 1,
        Error = 2,
        Warn = 4,
        Debug = 8,

        Default = Info | Error,
        All = Info | Error | Warn | Debug
    }
}
