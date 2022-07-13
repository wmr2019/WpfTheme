/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Common
*   文件名称 ：DependencyVariable.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 3:54:44 PM 
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
using System.Windows.Data;

namespace ThemeMetro.Common
{
    public sealed class DependencyVariable<T> : DependencyObject
    {
        public static DependencyProperty ValueProperty { get; } =
            DependencyProperty.Register("Value", typeof(T), typeof(DependencyVariable<T>));

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public void SetBinding(Binding binding)
        {
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        public void SetBinding(object dataContext, string propertyPath)
        {
            SetBinding(new Binding(propertyPath) { Source = dataContext });
        }
    }
}
