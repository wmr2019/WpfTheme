/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Common
*   文件名称 ：StyleColors.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 4:12:30 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Windows;
using System.Windows.Media;

namespace ThemeMetro.Common
{
    public static class StyleColors
    {
        public static SolidColorBrush GetWindowActiveBorderBrush() => Application.Current.FindResource("Blue0006") as SolidColorBrush;

        public static SolidColorBrush GetWindowInactiveBorderBrush() => Application.Current.FindResource("Black0009") as SolidColorBrush;
    }
}
