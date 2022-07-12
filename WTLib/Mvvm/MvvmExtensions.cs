﻿namespace WTLib.Mvvm
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public static class MvvmExtensions
    {
        public static object CreateDefault(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (!type.GetTypeInfo().IsValueType)
            {
                return null;
            }

            if (Nullable.GetUnderlyingType(type) != null)
                return null;

            return Activator.CreateInstance(type);
        }

        public static bool ConvertToBooleanCore(this object result)
        {
            if (result == null)
                return false;

            if (result is string s)
                return !string.IsNullOrEmpty(s);

            if (result is bool b)
                return b;

            var resultType = result.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        public static object MakeSafeValueCore(this Type propertyType, object value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType == typeof(string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.GetTypeInfo().IsEnum)
                {
                    safeValue = value is string s ? Enum.Parse(propertyType, s, true) : Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    safeValue = underlyingType == typeof(bool) ? value.ConvertToBooleanCore() : ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else
                {
                    safeValue = ErrorMaskedConvert(value, propertyType, CultureInfo.CurrentUICulture);
                }
            }
            return safeValue;
        }

        private static object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
        {
            try
            {
                return Convert.ChangeType(value, type, cultureInfo);
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
