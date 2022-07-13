/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：FocusWhenSelectedBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 8:36:13 PM 
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
    public class FocusWhenSelectedBehavior
    {
        public static DependencyProperty FocusWhenSelectedProperty = DependencyProperty.RegisterAttached(
            "FocusWhenSelected",
            typeof(bool),
            typeof(FocusWhenSelectedBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnFocusWhenSelectedChanged)));

        public static bool GetFocusWhenSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusWhenSelectedProperty);
        }

        public static void SetFocusWhenSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusWhenSelectedProperty, value);
        }

        private static void OnFocusWhenSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ListBox listbox)
            {
                SelectionChangedEventHandler handler = (s, e) =>
                {
                    try
                    {
                        listbox.UpdateLayout();
                        var listBoxItem = (ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(listbox.SelectedItem);
                        if (listBoxItem != null)
                            listBoxItem.Focus();
                    }
                    catch { }
                };
                listbox.SelectionChanged -= handler;
                listbox.SelectionChanged += handler;
                listbox.Unloaded += delegate
                {
                    listbox.SelectionChanged -= handler;
                };
            }
        }
    }
}
