using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace WTLib.Utils
{
    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    public class TimeNative : object
    {
        [DllImport("kernel32.dll", EntryPoint = "GetLocalTime", SetLastError = true)]
        private static extern void GetLocalTimeNative([Out, MarshalAs(UnmanagedType.LPStruct)] TimeNative timeNative);

        [DllImport("kernel32.dll", EntryPoint = "SystemTimeToFileTime", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemTimeToFileTimeNative([In, MarshalAs(UnmanagedType.LPStruct)] TimeNative timeNative, [Out, MarshalAs(UnmanagedType.I8)] out long fileTime);

        public static TimeNative GetLocalTime()
        {
            TimeNative native = new TimeNative();
            GetLocalTimeNative(native);
            return native;
        }

        public DateTime ToDateTime()
        {
            return SystemTimeToFileTimeNative(this, out long fileTime)
                ? DateTime.FromFileTimeUtc(fileTime)
                : DateTime.MinValue;
        }

        private static bool CheckIsLeapYear(int year)
        {
            return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
        }

        public override string ToString()
        {
            return ToDateTime().ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return ToDateTime().ToString(provider);
        }

        public string ToString(string format)
        {
            return ToDateTime().ToString(format);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return ToDateTime().ToString(format, provider);
        }
    }
}
