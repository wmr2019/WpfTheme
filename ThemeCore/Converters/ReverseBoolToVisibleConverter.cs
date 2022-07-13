/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：QiCheng.QCTrader.Controls.Converters
*   文件名称 ：NotNullToVisibleConverter.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 14:23:52 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeCore.Converters
{
    public class ReverseBoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return DependencyProperty.UnsetValue;
           
            return (bool)value ? Visibility.Collapsed :Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
