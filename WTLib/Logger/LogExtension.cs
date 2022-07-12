using System;

namespace QiCheng
{
    public static class LogExtension
    {
        public static void Log(this Exception ex, string message) => WTLib.Logger.Log.Trace.Error(ex.StackTrace, message);
    }
}
