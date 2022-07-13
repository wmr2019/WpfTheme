/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：DataGridTemplateColumnBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:25:28 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace ThemeMetro.Controls.Behaviors
{
    public class DataGridTemplateColumnBehavior
    {
        #region BindingPath

        public static DependencyProperty BindingPathProperty = DependencyProperty.RegisterAttached("BindingPath",
            typeof(string),
            typeof(DataGridTemplateColumnBehavior),
            new FrameworkPropertyMetadata(null));

        public static string GetBindingPath(DependencyObject target)
        {
            if (target == null)
                return null;

            if (target is DataGridTemplateColumn column && column.Header != null)
                return column.Header.ToString();

            return (string)target.GetValue(BindingPathProperty);
        }

        public static void SetBindingPath(DependencyObject target, string value)
        {
            target.SetValue(BindingPathProperty, value);
        }
        #endregion
    }
}
