namespace WTLib.Logger
{
    public class Log
    {
        public static ILogService Trace { get; }

        static Log()
        {
            Trace = new NlogService();
        }
    }
}
