/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：QiCheng.QCTrader.Controls.Converters
*   文件名称 ：SellColorConverter.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：1/13/2021 8:42:59 AM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ThemeMetro.Converters
{
    public class SellColorConverter : IValueConverter
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
                    ? new SolidColorBrush(Color.FromRgb(158, 158, 158))
                    : new SolidColorBrush(Color.FromRgb(0, 221, 0));
            }

            return new SolidColorBrush(Color.FromRgb(158, 158, 158));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(Color.FromRgb(158, 158, 158));
        }
    }
}
