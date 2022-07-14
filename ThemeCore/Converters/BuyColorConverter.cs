using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ThemeCore.Converters
{
    public class BuyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Color.FromRgb(158, 158, 158));
            }

            if (value is bool isBuy)
            {
                return isBuy
                    ? new SolidColorBrush(Color.FromRgb(225, 60, 60))
                    : new SolidColorBrush(Color.FromRgb(158, 158, 158));
            }

            return new SolidColorBrush(Color.FromRgb(158, 158, 158));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(Color.FromRgb(158, 158, 158));
        }
    }
}
