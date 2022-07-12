using System;

namespace WTLib.Utils
{
    public class ConvertUtil
    {
        public static int ToInt(object value)
        {
            if (value == null)
                return 0;
            if (value is int obj)
                return obj;

            var data = value.ToString().Trim();
            if (data.Contains("."))
            {
                if (decimal.TryParse(data, out decimal decValue))
                    return Convert.ToInt32(decValue);
                return 0;
            }

            int.TryParse(data, out int result);
            return result;
        }

        public static decimal ToDecimal(object value)
        {
            if (value == null)
                return 0;

            var data = value.ToString();
            if (decimal.TryParse(data, out decimal result))
            {
                return result;
            }
            else
            {
                if (data.Contains("E"))
                {
                    return ToDecimal(string.Format("{0:f9}", ToDouble(data)));
                }
            }

            return 0;
        }

        public static double ToDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            if (double.TryParse(value, out double result))
                return result;
            return 0;
        }

        public static DateTime ToDateTime(object value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }

            DateTime.TryParse(value.ToString().Trim(), out DateTime result);
            return result;
        }
    }
}
