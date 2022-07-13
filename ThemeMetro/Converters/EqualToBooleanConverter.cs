﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeMetro.Converters
{
    public class EqualToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) return DependencyProperty.UnsetValue;
            var curr = values[0];
            var other = values[1];
            return curr.Equals(other);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
